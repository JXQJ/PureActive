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
            catch (Exception ex)
            {
                logger?.LogError(ex, "{Method} Failed to Shutdown", memberName);

                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }
    }
}
