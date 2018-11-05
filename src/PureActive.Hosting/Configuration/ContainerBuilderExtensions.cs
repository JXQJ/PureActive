using Autofac;
using Ganss.XSS;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using PureActive.Archive.Abstractions.System;
using PureActive.Archive.System;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.Queue;
using PureActive.Core.Abstractions.Serialization;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Async;
using PureActive.Core.Serialization;
using PureActive.Core.System;
using PureActive.Email.Office365.Interfaces;
using PureActive.Email.Office365.Providers;
using PureActive.Hosting.Abstractions.System;
using PureActive.Queue.Hangfire.Queue;

namespace PureActive.Hosting.Configuration
{
    /// <summary>
    ///     Extension methods for building the IOC container on application start.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        ///     Registers Office365 mail provider
        /// </summary>
        public static void RegisterOffice365MailProvider(
            this ContainerBuilder builder,
            IConfiguration configuration)
        {
            builder.RegisterInstance
                (
                    CreateOffice365MailProvider(
                        configuration["Email:EmailAddress"],
                        configuration["Email:EmailDisplayName"],
                        configuration["Email:Providers:Office365:UserName"],
                        configuration["Email:Providers:Office365:Password"]
                    )
                )
                .As<IEmailProvider>()
                .As<IEmailSender>();
        }


        /// <summary>
        ///     Creates a new Office365 mail provider.
        /// </summary>
        private static IEmailProvider CreateOffice365MailProvider(
            string fromEmail, string fromName, string userName, string password)
        {
            // string fromEmail, string fromName, string userName, string password

            return new Office365EmailProvider(fromEmail, fromName, userName, password);
        }

        /// <summary>
        ///     Registers security-related dependencies.
        /// </summary>
        public static void RegisterSecurity(this ContainerBuilder builder)
        {
            builder.RegisterType<HtmlSanitizer>().As<IHtmlSanitizer>().InstancePerLifetimeScope();
        }

        /// <summary>
        ///     Registers container for common services
        /// </summary>
        public static void RegisterCommonServices(this ContainerBuilder builder)
        {
            builder.RegisterType<CommonServices.CommonServices>().As<ICommonServices>().InstancePerLifetimeScope();
        }

        /// <summary>
        ///     Registers classes that interact with the system.
        /// </summary>
        public static void RegisterSystem(this ContainerBuilder builder)
        {
            builder.RegisterType<ArchiveFactory>().As<IArchiveFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProcessRunner>().As<IProcessRunner>().InstancePerLifetimeScope();
            //builder.RegisterType<FileSystem>().As<IFileSystem>().InstancePerLifetimeScope();
            builder.RegisterType<TimeProvider>().As<ITimeProvider>().InstancePerLifetimeScope();
        }

        /// <summary>
        ///     Registers the operation runner.
        /// </summary>
        public static void RegisterOperationRunner(this ContainerBuilder builder)
        {
            builder.RegisterType<OperationRunner>().As<IOperationRunner>().InstancePerLifetimeScope();
        }

        /// <summary>
        ///     Registers the operation runner.
        /// </summary>
        public static void RegisterJobQueueClient(this ContainerBuilder builder)
        {
            builder.RegisterType<JobQueueClient>().As<IJobQueueClient>().InstancePerLifetimeScope();
        }

        public static void RegisterJsonSerialization(this ContainerBuilder builder, ITypeMapCollection typeMaps)
        {
            builder.RegisterType<JsonSettingsProvider>().As<IJsonSettingsProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ModelSerializer>().As<IJsonSerializer>().InstancePerLifetimeScope();
            builder.RegisterInstance(typeMaps).As<ITypeMapCollection>();
        }
    }
}