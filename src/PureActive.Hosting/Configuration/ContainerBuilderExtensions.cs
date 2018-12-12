// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-03-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-05-2018
// ***********************************************************************
// <copyright file="ContainerBuilderExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Autofac;
using Ganss.XSS;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using PureActive.Archive.Abstractions.System;
using PureActive.Archive.System;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.Serialization;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Async;
using PureActive.Core.Serialization;
using PureActive.Core.System;
using PureActive.Email.Office365.Interfaces;
using PureActive.Email.Office365.Providers;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Settings;

namespace PureActive.Hosting.Configuration
{
    /// <summary>
    /// Extension methods for building the IOC container on application start.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers Office365 mail provider
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
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
        /// Creates a new Office365 mail provider.
        /// </summary>
        /// <param name="fromEmail">From email.</param>
        /// <param name="fromName">From name.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>IEmailProvider.</returns>
        private static IEmailProvider CreateOffice365MailProvider(
            string fromEmail, string fromName, string userName, string password)
        {
            // string fromEmail, string fromName, string userName, string password

            return new Office365EmailProvider(fromEmail, fromName, userName, password);
        }

        /// <summary>
        /// Registers security-related dependencies.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterSecurity(this ContainerBuilder builder)
        {
            builder.RegisterType<HtmlSanitizer>().As<IHtmlSanitizer>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers container for common services
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterCommonServices(this ContainerBuilder builder)
        {
            builder.RegisterType<CommonServices.CommonServices>().As<ICommonServices>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers classes that interact with the system.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterSystem(this ContainerBuilder builder)
        {
            builder.RegisterType<ArchiveFactory>().As<IArchiveFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ProcessRunner>().As<IProcessRunner>().InstancePerLifetimeScope();
            //builder.RegisterType<FileSystem>().As<IFileSystem>().InstancePerLifetimeScope();
            builder.RegisterType<TimeProvider>().As<ITimeProvider>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers classes that interact with the system.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="webAppSettings">Configuration section for WebSettings</param>
        public static void RegisterWebAppSettings(this ContainerBuilder builder, IConfigurationSection webAppSettings)
        {
            builder.RegisterInstance(ConfigurationSectionExtensions.GetHostName(webAppSettings)).As<WebAppHost>();
            builder.RegisterInstance(ConfigurationSectionExtensions.GetEmailAddress(webAppSettings)).As<WebAppEmail>();
            builder.RegisterInstance(ConfigurationSectionExtensions.GetErrorSettings(webAppSettings))
                .As<ErrorSettings>();
        }

        /// <summary>
        /// Registers the operation runner.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterOperationRunner(this ContainerBuilder builder)
        {
            builder.RegisterType<OperationRunner>().As<IOperationRunner>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers the json serialization.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="typeMaps">The type maps.</param>
        /// <autogeneratedoc />
        public static void RegisterJsonSerialization(this ContainerBuilder builder, ITypeMapCollection typeMaps)
        {
            builder.RegisterType<JsonSettingsProvider>().As<IJsonSettingsProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ModelSerializer>().As<IJsonSerializer>().InstancePerLifetimeScope();
            builder.RegisterInstance(typeMaps).As<ITypeMapCollection>();
        }
    }
}