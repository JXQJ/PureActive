// ***********************************************************************
// Assembly         : PureActive.Queue.Hangfire.IntegrationTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="QueueHangfireIntegrationTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Hangfire;
using Hangfire.SQLite;
using Hangfire.Storage;
using PureActive.Core.Abstractions.Queue;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Queue.Hangfire.Queue;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace PureActive.Queue.Hangfire.IntegrationTests
{
    /// <summary>
    /// Class QueueHangfireIntegrationTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{QueueHangfireIntegrationTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{QueueHangfireIntegrationTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Integration")]
    public class QueueHangfireIntegrationTests : TestBaseLoggable<QueueHangfireIntegrationTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueHangfireIntegrationTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public QueueHangfireIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        static QueueHangfireIntegrationTests()
        {
            var fileSystem = new FileSystem(typeof(QueueHangfireIntegrationTests));

            var databaseFolderPath = TestBackgroundJobServer.GetDatabasePath(fileSystem, ServiceHost.HangfireTest);

            if (fileSystem.FolderExists(databaseFolderPath))
            {
                fileSystem.DeleteFolder(databaseFolderPath);
            }
        }

        [ExcludeFromCodeCoverage]
        public class HangfireJob
        {
            public string TestValue { get; }

            public HangfireJob(string testValue)
            {
                TestValue = testValue;
            }
        }

        public interface IHangFireTestService
        {
            Task ExecuteHangfireTestJobAsync(HangfireJob hangfireJob);

            Task ExecuteHangfireInfiniteJobAsync(HangfireJob hangfireJob);
        }

        [ExcludeFromCodeCoverage]
        public class HangFireTestService : IHangFireTestService
        {
            /// <inheritdoc />
            public Task ExecuteHangfireTestJobAsync(HangfireJob hangfireJob)
            {
                return Task.Run( () => Console.WriteLine($"Job TestValue: {hangfireJob.TestValue}")
                    );
            }

            /// <inheritdoc />
            public Task ExecuteHangfireInfiniteJobAsync(HangfireJob hangfireJob)
            {
                return Task.Delay(-1);
            }
        }

        [ExcludeFromCodeCoverage]
        private class TestBackgroundJobServer : IDisposable
        {
            public JobStorage JobStorage { get; }

            public BackgroundJobServer BackgroundJobServer { get; }

            public IMonitoringApi MonitoringApi => JobStorage?.GetMonitoringApi();

            private readonly string _memberName;

            private readonly IFileSystem _fileSystem;

            private readonly ServiceHost _serviceHost;

            public TestBackgroundJobServer(IFileSystem fileSystem, ServiceHost serviceHost, int startupDelay = 10000)
            {
                _memberName = "QueueHangfireIntegrationTests";
                _fileSystem = fileSystem;
                _serviceHost = serviceHost;

                // CleanupDatabase();

                var builder = new ContainerBuilder();

                builder.RegisterType<HangFireTestService>().As<IHangFireTestService>().InstancePerLifetimeScope();

                JobStorage = new SQLiteStorage(GetConnectionString());
                GlobalConfiguration.Configuration.UseStorage(JobStorage);
                GlobalConfiguration.Configuration.UseAutofacActivator(builder.Build());

                BackgroundJobServer = new BackgroundJobServer(JobStorage);

                BackgroundJobServer.Should().NotBeNull();
                Task.Delay(startupDelay).Wait();
            }

            public static string GetDatabasePath(IFileSystem fileSystem, ServiceHost serviceHost)
            {
                var databasePath = $"{fileSystem.DataFolderPath()}/{serviceHost}";
                fileSystem.CreateFolder(databasePath);

                return databasePath;
            }

            private string GetDatabasePath() => GetDatabasePath(_fileSystem, _serviceHost);

            private string GetDatabaseFileName()
            {
               return $"{GetDatabasePath()}/{ _memberName}.db";
            }

            private string GetConnectionString()
            {
                return $"Data Source={GetDatabaseFileName()};";
            }

            public bool DeleteBackgroundJob(string jobId)
            {
                return BackgroundJob.Delete(jobId);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                BackgroundJobServer.Dispose();
            }
        }

        /// <summary>
        /// Defines the test method that runs a Background Hangfire server
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public async Task QueueHangfire_BackgroundServer_Run()
        {
            using (var server = new TestBackgroundJobServer(FileSystem, ServiceHost.HangfireTest))
            {
                const string testValue = "TestValue";

                var hangFireJob = new HangfireJob(testValue);

                var jobIdString = BackgroundJob.Enqueue<IHangFireTestService>(service =>
                    service.ExecuteHangfireTestJobAsync(hangFireJob));

                jobIdString.Should().NotBeNull();

                var jobStatus = await JobQueueClient.WaitForJobToComplete(server.MonitoringApi, jobIdString, 30 * 1000, CancellationToken.None);
                jobStatus.State.Should().Be(JobState.Succeeded);

                server.DeleteBackgroundJob(jobIdString).Should().BeTrue();
            }
        }

        /// <summary>
        /// Defines the test method that WaitForJobToComplete with unknown job
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public async Task QueueHangfire_WaitForJobToComplete_JobNotFound()
        {
            using (var server = new TestBackgroundJobServer(FileSystem, ServiceHost.HangfireTest))
            {
                server.Should().NotBeNull();

                var jobStatus = await JobQueueClient.WaitForJobToComplete(server.MonitoringApi, "-1", 30, CancellationToken.None);
                jobStatus.State.Should().Be(JobState.NotFound);
            }
        }

        /// <summary>
        /// Defines the test method that WaitForJobToComplete with Infinite timeout and Cancellation
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void QueueHangfire_WaitForJobToComplete_Infinite_Cancel()
        {
            using (var server = new TestBackgroundJobServer(FileSystem, ServiceHost.HangfireTest))
            {
                const string testValue = "TestValue";

                var hangFireJob = new HangfireJob(testValue);

                var jobIdString = BackgroundJob.Enqueue<IHangFireTestService>(service =>
                    service.ExecuteHangfireInfiniteJobAsync(hangFireJob));

                var cts = new CancellationTokenSource();

                jobIdString.Should().NotBeNull();
                int.TryParse(jobIdString, out var jobId).Should().BeTrue();
                jobId.Should().BeGreaterThan(0);

                var monitoringApi = server.MonitoringApi;

                cts.CancelAfter(1000);

                Func<Task<JobStatus>> fx = async () =>
                    await JobQueueClient.WaitForJobToComplete(monitoringApi, jobIdString, -1, cts.Token);

                fx.Should().Throw<TaskCanceledException>();

                BackgroundJob.Delete(jobIdString).Should().BeTrue();
            }
        }

        /// <summary>
        /// Defines the test method that WaitForJobToComplete with Infinite timeout and Cancellation
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void QueueHangfire_WaitForJobToComplete_ZeroTimeout()
        {
            using (var server = new TestBackgroundJobServer(FileSystem, ServiceHost.HangfireTest))
            {
                server.Should().NotBeNull();

                const string testValue = "TestValue";

                var hangFireJob = new HangfireJob(testValue);

                var jobIdString = BackgroundJob.Enqueue<IHangFireTestService>(service =>
                    service.ExecuteHangfireInfiniteJobAsync(hangFireJob));

                CancellationTokenSource cts = new CancellationTokenSource();

                jobIdString.Should().NotBeNull();
                int.TryParse(jobIdString, out var jobId).Should().BeTrue();
                jobId.Should().BeGreaterThan(0);

                cts.CancelAfter(1000);

                var monitoringApi = server.MonitoringApi;

                Func<Task<JobStatus>> fx = async () =>
                    await JobQueueClient.WaitForJobToComplete(monitoringApi, jobIdString, 500, cts.Token);

                fx.Should().Throw<TaskCanceledException>();

                BackgroundJob.Delete(jobIdString).Should().BeTrue();
            }
        }
    }
}