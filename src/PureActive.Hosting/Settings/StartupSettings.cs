using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PureActive.Core.Abstractions.System;
using PureActive.Hosting.Abstractions.Settings;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Configuration;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Hosting.Settings
{
    public abstract class StartupSettings : IStartupSettings
    {
        /// <summary>
        ///     The DI container for the application.
        /// </summary>
        protected IContainer Container;

        protected StartupSettings(IConfiguration configuration, IHostingEnvironment hostingEnvironment,
            IPureLoggerFactory loggerFactory, ServiceHost serviceHost, IFileSystem fileSystem, IOperatingSystem operatingSystem, ServiceHostConfig serviceHostConfig = ServiceHostConfig.Kestrel,
            ServiceDatabaseConfig serviceDatabaseConfig = ServiceDatabaseConfig.LocalHost)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));

            ServiceHost = serviceHost;
            ServiceHostConfig = serviceHostConfig;
            ServiceDatabaseConfig = serviceDatabaseConfig;
        }

        // Public Interfaces
        public ServiceHost ServiceHost { get; internal set; }
        public ServiceHostConfig ServiceHostConfig { get; internal set; }
        public ServiceDatabaseConfig ServiceDatabaseConfig { get; internal set; }
        public IConfiguration Configuration { get; internal set; }
        public IHostingEnvironment HostingEnvironment { get; internal set; }
        public IPureLoggerFactory LoggerFactory { get; internal set; }
        public IFileSystem FileSystem { get; internal set; }
        public IOperatingSystem OperatingSystem { get; internal set; }

        public ContainerBuilder RegisterSharedServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.RegisterSecurity();
            builder.RegisterOffice365MailProvider(Configuration);
            builder.RegisterSystem();
            builder.RegisterOperationRunner();
            builder.RegisterCommonServices();

            return builder;
        }

        public IServiceProvider BuildContainer(ContainerBuilder builder, IServiceCollection services)
        {
            builder.Populate(services);
            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        /// <summary>
        ///     Returns the given configuration section.
        /// </summary>
        public IConfigurationSection GetSection(string sectionName)
        {
            return Configuration.GetSection(sectionName);
        }

        public string GetConnectionString(ServiceHost serviceHost)
        {
            return GetConnectionString(Configuration, FileSystem, serviceHost);
        }

        public abstract void ApplyDatabaseMigrations(IApplicationBuilder app);

        private static string DatabaseName(ServiceHost serviceHost)
        {
            return serviceHost.ToString() + ".db";
        }

        public static string GetConnectionString(IConfiguration configuration, IFileSystem fileSystem,
            ServiceHost serviceHost)
        {
            var databasePath = fileSystem.DataFolderPath() + DatabaseName(serviceHost);

            return $"Data Source={databasePath};";
        }
    }
}