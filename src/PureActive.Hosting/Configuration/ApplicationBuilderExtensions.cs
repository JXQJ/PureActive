using Autofac;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;

namespace PureActive.Hosting.Configuration
{
    /// <summary>
    ///     An authorization filter for the Hangfire dashboard.
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        ///     The user provider.
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public HangfireAuthorizationFilter(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     Authorizes access to the hangfire dashboard.
        /// </summary>
        public bool Authorize(DashboardContext context)
        {
            // TODO: Make it Admin Only once roles established

            return _container != null && context.GetHttpContext().User.Identity.IsAuthenticated;
        }
    }

    /// <summary>
    ///     Extension methods for building the IOC container on application start.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Registers dependencies for CSClassroom services.
        /// </summary>
        public static void UseHangfireQueueDashboard(this IApplicationBuilder builder, IContainer container)
        {
            builder.UseHangfireDashboard("/PureActiveQueue", new DashboardOptions
            {
                Authorization = new[] {new HangfireAuthorizationFilter(container)}
            });
        }
    }
}