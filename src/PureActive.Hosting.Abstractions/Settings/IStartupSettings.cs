using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Hosting.Abstractions.Settings
{
    public interface IStartupSettings
    {
        ServiceHost ServiceHost { get; }
        ServiceHostConfig ServiceHostConfig { get; }
        ServiceDatabaseConfig ServiceDatabaseConfig { get; }
        IConfiguration Configuration { get; }
        IHostingEnvironment HostingEnvironment { get; }
        IPureLoggerFactory LoggerFactory { get; }

        ContainerBuilder RegisterSharedServices(IServiceCollection services);

        IServiceProvider BuildContainer(ContainerBuilder builder, IServiceCollection services);

        IConfigurationSection GetSection(string sectionName);

        void ApplyDatabaseMigrations(IApplicationBuilder app);

        string GetConnectionString(ServiceHost serviceHost);
    }
}