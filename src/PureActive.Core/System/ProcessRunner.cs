using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Core.System
{
    /// <summary>
    ///     Runs a process.
    /// </summary>
    public class ProcessRunner : IProcessRunner
    {
        /// <summary>
        ///     The logger.
        /// </summary>
        private readonly IPureLogger _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ProcessRunner(IPureLogger<ProcessRunner> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Runs a process, optionally killing the process if it has not completed
        ///     in the given timeout. Returns the combined contents of stdout/stderr,
        ///     along with whether the job completed in the given timeout.
        /// </summary>
        public async Task<ProcessResult> RunProcessAsync(
            string path,
            string[] args,
            TimeSpan? timeout)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (args == null) throw new ArgumentNullException(nameof(args));

            var output = new ConcurrentQueue<string>();
            var processTcs = new TaskCompletionSource<bool>();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = string.Join(" ", args),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },

                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) => { output.Enqueue(e.Data); };

            process.ErrorDataReceived += (sender, e) => { output.Enqueue(e.Data); };

            process.Exited += (sender, e) => { processTcs.TrySetResult(true); };

            _logger?.LogInformation("Starting process {processName} with arguments {arguments}.",
                process.StartInfo.FileName,
                process.StartInfo.Arguments);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (timeout != null)
                await Task.WhenAny(processTcs.Task, Task.Delay(timeout.Value));
            else
                await processTcs.Task;

            var completed = process.HasExited;
            if (!completed) process.Kill();

            var cRetries = 5;

            // Add delay to allow output to be processed
            while (output.Count == 0 && cRetries-- > 0)
            {
                // Wait 100 ms
                await Task.Delay(100);
            }

            var outputStr = string.Join("\n", output);

            var truncatedOutputStr = outputStr.Length > 1000
                ? outputStr.Substring(outputStr.Length - 1000)
                : outputStr;

            _logger?.LogInformation
            (
                "Process {processName} with arguments {arguments} finished with status {status}",
                process.StartInfo.FileName,
                process.StartInfo.Arguments,
                completed ? "Completed" : "Timeout"
            );

            _logger?.LogTrace("Process {processName} with arguments {arguments} truncated output: \n{output}",
                process.StartInfo.FileName,
                process.StartInfo.Arguments, 
                truncatedOutputStr);

            return new ProcessResult(completed, outputStr);
        }
    }
}