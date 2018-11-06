using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Logger.Provider.Serilog.Extensions;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;

namespace PureActive.Network.Services.ArpService
{
    public class ArpService : BackgroundServiceInternal<ArpService>, IArpService
    {
        // ILoggable Interface
        private readonly IPingService _pingService;
        
        private readonly Dictionary<PhysicalAddress, ArpItem> _physical2ArpItem = new Dictionary<PhysicalAddress, ArpItem>();
        private readonly Dictionary<IPAddress, ArpItem> _ipAddress2ArpItem = new Dictionary<IPAddress, ArpItem>();
        private readonly object _updateLock = new object();

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 3, 0);    // 3 minutes

        public int Count => _physical2ArpItem.Count;

        private readonly int _processExitDelay = 100;

        public ArpRefreshStatus LastArpRefreshStatus { get; internal set; }
        public DateTimeOffset LastUpdated { get; internal set; }

        public ArpService(ICommonServices commonServices, IPingService pingService, IApplicationLifetime applicationLifetime = null) :
            base(commonServices, applicationLifetime, ServiceHost.ArpService)
        {
            _pingService = pingService;
        }

        /// <summary>
        /// The number of times we will attempt run arp command
        /// </summary>
        private const int ArpRetryAttempts = 3;
        private readonly TimeSpan _retryAttemptDelay = TimeSpan.FromSeconds(1);

        private async Task<ProcessResult> RunArpAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var arpCommandPath = CommonServices.FileSystem.ArpCommandPath();
            var args = new[] {$"-a"};

            var processResult = await CommonServices.OperationRunner.RetryOperationIfNeededAsync(
                async () => await CommonServices.ProcessRunner.RunProcessAsync(arpCommandPath, args, timeout),
                exception => true,
                ArpRetryAttempts,
                _retryAttemptDelay,
                false,
                cancellationToken
            );

            if (!processResult.Completed || string.IsNullOrWhiteSpace(processResult.Output))
            {
                Logger.LogError("RunArpAsync failed to produce output");
            }

            return processResult;
        }

        public void ClearArpCache()
        {
            lock (_updateLock)
            {
                _physical2ArpItem.Clear();
                _ipAddress2ArpItem.Clear();
            }
        }

        public Task RefreshArpCacheAsync(CancellationToken stoppingToken, bool clearCache)
        {
            if (clearCache)
                ClearArpCache();

            var executingTask = ExecuteAsync(stoppingToken);

            return executingTask.IsCompleted ? executingTask : Task.CompletedTask;
        }

        public Task RefreshArpCacheAsync(bool clearCache)
        {
            return RefreshArpCacheAsync(CancellationToken.None, clearCache);
        }

        private ArpItem _UpdateArpItem(PhysicalAddress physicalAddress, IPAddress ipAddress, DateTimeOffset timestamp)
        {
            lock (_updateLock)
            {
                if (_physical2ArpItem.TryGetValue(physicalAddress, out var arpItem))
                {
                    // Update IP Address
                    if (!arpItem.IPAddress.Equals(ipAddress))
                    {
                        // Remove old entry and add new
                        _ipAddress2ArpItem.Remove(arpItem.IPAddress);

                        arpItem.IPAddress = ipAddress;
                        _ipAddress2ArpItem.Add(ipAddress, arpItem);
                    }

                    // Update cache value based on this session
                    arpItem.CreatedTimestamp = timestamp;
                }
                else
                {
                    // Create new entry
                    arpItem = new ArpItem(physicalAddress, ipAddress, timestamp);

                    // Add to both Dictionaries
                    _physical2ArpItem.Add(physicalAddress, arpItem);
                    _ipAddress2ArpItem.Add(ipAddress, arpItem);
                }

                return arpItem;
            }
        }

        private ArpRefreshStatus _ProcessArpOutput(string arpResults, CancellationToken cancellationToken)
        {     
            var timestamp = DateTimeOffset.Now;

            Regex regexIpAddress = new Regex(@"\b([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\b", RegexOptions.IgnoreCase);
            Regex regexPhysicalAddress = new Regex(@"(?<![\w\-:])[0-9A-F]{1,2}([-:]?)(?:[0-9A-F]{1,2}\1){4}[0-9A-F]{1,2}(?![\w\-:])", RegexOptions.IgnoreCase);

            using (var stringReader = new StringReader(arpResults))
            {
                string inputLine;

                while ((inputLine = stringReader.ReadLine()) != null)
                {
                    if (inputLine.Length == 0)
                        continue;

                    if (cancellationToken.IsCancellationRequested)
                        return ArpRefreshStatus.Cancelled;

                    try
                    {
                        var ipAddressString = regexIpAddress.Match(inputLine).Value;
                        var physicalAddressString = regexPhysicalAddress.Match(inputLine).Value;

                        if (!string.IsNullOrWhiteSpace(ipAddressString) &&
                            !string.IsNullOrWhiteSpace(physicalAddressString))
                        {
                            var physicalAddress = PhysicalAddressExtensions.NormalizedParse(physicalAddressString);
                            var ipAddress = IPAddress.Parse(ipAddressString);

                            var arpItem = _UpdateArpItem(physicalAddress, ipAddress, timestamp);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError(ex, "ArpOutput parsing error with line {ArpOutputLine}", inputLine);
                    }                                       
                }
            }

            return ArpRefreshStatus.Processed;
        }

        private async Task RefreshInternalAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var arpRefreshStatus = ArpRefreshStatus.Running;

            var processResult = await RunArpAsync(timeout, cancellationToken);

            await Task.Delay(_processExitDelay, cancellationToken);

            if (processResult.Completed  && !cancellationToken.IsCancellationRequested)
            {
                arpRefreshStatus = _ProcessArpOutput(processResult.Output, cancellationToken);
            }

            // if Processed succeeded then we're completed
            if (arpRefreshStatus == ArpRefreshStatus.Processed)
            {
                arpRefreshStatus = ArpRefreshStatus.Completed;
            }

            LastUpdated = DateTimeOffset.Now;
            LastArpRefreshStatus = arpRefreshStatus;

            // Update ServiceHostStatus to Running
            if (ServiceHostStatus == ServiceHostStatus.StartPending)
                ServiceHostStatus = ServiceHostStatus.Running;

            using (this.With(LogLevel.Debug))
            {
                Logger?.LogDebug("{ServiceHost} Refresh Finished with Status: {ArpRefreshStatus}, Devices Discovered: {ArpDeviceCount}", ServiceHost, arpRefreshStatus, Count);
            }
        }

        protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RefreshInternalAsync(Timeout, stoppingToken);
        }

     public IEnumerator<ArpItem> GetEnumerator()
        {
            return _ipAddress2ArpItem.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public async Task<ArpItem> GetArpItemAsync(IPAddress ipAddress, CancellationToken cancellationToken)
        {
            ArpItem arpItem;

            lock (_updateLock)
            {
                if (_ipAddress2ArpItem.TryGetValue(ipAddress, out arpItem))
                    return arpItem;
            }

            var pingReply = await _pingService.PingIpAddressAsync(ipAddress);

            await RefreshInternalAsync(Timeout, cancellationToken);

            lock (_updateLock)
            {
                _ipAddress2ArpItem.TryGetValue(ipAddress, out arpItem);
            }

            return arpItem;
        }

         public async Task<ArpItem> GetArpItemAsync(IPAddress ipAddress) => await GetArpItemAsync(ipAddress, CancellationToken.None);

        public async Task<PhysicalAddress> GetPhysicalAddressAsync(IPAddress ipAddress, CancellationToken cancellationToken)
        {
            var arpItem = await GetArpItemAsync(ipAddress, cancellationToken);

            return arpItem?.PhysicalAddress ?? PhysicalAddress.None;
        }

        public async Task<PhysicalAddress> GetPhysicalAddressAsync(IPAddress ipAddress) => await GetPhysicalAddressAsync(ipAddress, CancellationToken.None);
        public PhysicalAddress GetPhysicalAddress(IPAddress ipAddress) => GetPhysicalAddressAsync(ipAddress).Result;

        public IPAddress GetIPAddress(PhysicalAddress physicalAddress, bool refreshCache)
        {
            lock (_updateLock)
            {
                if (_physical2ArpItem.TryGetValue(physicalAddress, out var arpItem))
                    return arpItem.IPAddress;
            }

            if (refreshCache)
            {
                RefreshInternalAsync(Timeout, CancellationToken.None).Wait();

                lock (_updateLock)
                {
                    if (_physical2ArpItem.TryGetValue(physicalAddress, out var arpItem))
                        return arpItem.IPAddress;
                }
            }

            return IPAddress.None;
        }

        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel, LoggableFormat loggableFormat)
        {
            var logProperties = new List<IPureLogPropertyLevel>
            {
                {new PureLogPropertyLevel("DiscoveredDevices", _physical2ArpItem.Count, LogLevel.Information)},
                {new PureLogPropertyLevel("ArpLastRefreshStatus", LastArpRefreshStatus, LogLevel.Information)},
                {new PureLogPropertyLevel("ArpLastUpdated", LastUpdated, LogLevel.Information)},
            };

            if (logLevel <= LogLevel.Debug)
            {
                foreach (var arpItem in this)
                {
                    logProperties.Add(new PureLogPropertyLevel(arpItem.IPAddress.ToString(), arpItem.PhysicalAddress.ToDashString(), LogLevel.Information));
                }
            }

            return logProperties.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }
    }
}
