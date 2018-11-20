using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Core.Extensions
{
    public static class TaskExtensions
    {
        public static Task WaitForTasks(this List<Task> tasks, CancellationToken cancellationToken,
            IPureLogger logger = null, [CallerMemberName] string memberName = "")
        {
            Task t = Task.WhenAll(tasks);

            try
            {
                t.Wait(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                logger?.LogError("Task {Method} timed out", memberName);

                t = Task.FromException(ex);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Task {Method} threw an exception", memberName);

                t = Task.FromException(ex);
            }

            return t;
        }
    }
}