using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PureActive.Core.Abstractions.Queue;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Serialization;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Configuration;
using PureActive.Hosting.Hangfire.Configuration;
using PureActive.Hosting.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using Serilog.Events;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace PureActive.Hosting.Hangfire.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class QueueHangfireHostingIntegrationUnitTests
    {
        [ExcludeFromCodeCoverage]
        private class StartupTestHangFireBadConnectionString : StartupSettings
        {
            public StartupTestHangFireBadConnectionString(IConfiguration configuration, IHostingEnvironment hostingEnvironment,
                IPureLoggerFactory loggerFactory, IFileSystem fileSystem, IOperatingSystem operatingSystem) :
                base(configuration, hostingEnvironment, loggerFactory, ServiceHost.StartupSettingsTest, fileSystem,
                    operatingSystem)
            {

            }

            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                if (services == null) throw new ArgumentNullException(nameof(services));

                var builder = RegisterSharedServices(services);

                // Register Shared Services
                builder.RegisterJobQueueClient();
                builder.RegisterWebAppSettings(GetSection("StartupTestService"));

                builder.RegisterJsonSerialization(new TypeMapCollection());

                services.AddHangfireQueue("Bad", LoggerFactory);

                return BuildContainer(builder, services);
            }

            /// <inheritdoc />
            public override void ApplyDatabaseMigrations(IApplicationBuilder app)
            {

            }


            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                if (app == null) throw new ArgumentNullException(nameof(app));
                if (env == null) throw new ArgumentNullException(nameof(env));

                app.UseHangfireQueueDashboard(Container);
            }

        }
        private static readonly string LogFileName = "hangfire-hosting-test.log";

        private static IWebHost BuildWebHost<TStartup>(string[] args) where TStartup : class
        {
            var webHostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddAppSettings(env);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                });


            webHostBuilder
                .UseSystemSettings(LogFileName, IncludeLogEvent)
                .UseApplicationInsights()
                .UseStartup<TStartup>();


            return webHostBuilder.Build();
        }

        [ExcludeFromCodeCoverage]
        private static bool IncludeLogEvent(LogEvent logEvent)
        {
            return true;
        }


        [ExcludeFromCodeCoverage]
        private class StartupHangfireTest : StartupSettings
        {
            /// <inheritdoc />
            public StartupHangfireTest(IConfiguration configuration, IHostingEnvironment hostingEnvironment,
                IPureLoggerFactory loggerFactory, IFileSystem fileSystem, IOperatingSystem operatingSystem) :
                base(configuration, hostingEnvironment, loggerFactory, ServiceHost.HangfireTest, fileSystem,
                    operatingSystem)
            {

            }

            /// <inheritdoc />
            public override void ApplyDatabaseMigrations(IApplicationBuilder app)
            {

            }

            // This method gets called by the runtime. Use this method to add services to the container.
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                if (services == null) throw new ArgumentNullException(nameof(services));

                var builder = RegisterSharedServices(services);

                // Register JobQueue client
                builder.RegisterJobQueueClient();
                builder.RegisterType<HangFireTestService>().As<IHangFireTestService>();

                services.AddHangfireQueue(GetConnectionString(ServiceHost.Hangfire), LoggerFactory);

                return BuildContainer(builder, services);
            }

            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                if (app == null) throw new ArgumentNullException(nameof(app));
                if (env == null) throw new ArgumentNullException(nameof(env));

                app.UseHangfireQueueDashboard(Container);
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
        }

        [ExcludeFromCodeCoverage]
        public class HangFireTestService : IHangFireTestService
        {
            /// <inheritdoc />
            public Task ExecuteHangfireTestJobAsync(HangfireJob hangfireJob)
            {
                return Task.Run(() => Console.WriteLine($"Job TestValue: {hangfireJob.TestValue}")
                );
            }
        }

        [Fact]
        public void QueueHangfire_Startup_BadConnectionString()
        {
            var webHost = BuildWebHost<StartupTestHangFireBadConnectionString>(new string[0]);
            webHost.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IWebHost>();

            var cts = new CancellationTokenSource();

            cts.CancelAfter(500);
            Func<Task> fx = () => webHost.RunAsync(cts.Token);
            fx.Should().Throw<DependencyResolutionException>();

        }


        /// <summary>
        /// Defines the test method Runs Hangfire Service and cancels
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public async Task QueueHangfire_Run_Cancel()
        {
            var webHost = BuildWebHost<StartupHangfireTest>(new string[0]);
            webHost.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IWebHost>();

            var cts = new CancellationTokenSource();

            cts.CancelAfter(500);
            await webHost.RunAsync(cts.Token);
        }


        /// <summary>
        /// Defines the test method Runs Hangfire Service
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public async Task QueueHangfire_Run_WaitForJobToComplete_Cancelled()
        {
            var webHost = BuildWebHost<StartupHangfireTest>(new string[0]);
            webHost.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IWebHost>();

            var jobQueueClient = webHost.Services.GetService(typeof(IJobQueueClient)).As<IJobQueueClient>();
            jobQueueClient.Should().NotBeNull().And.Subject.Should().BeAssignableTo<IJobQueueClient>();

            const string testValue = "TestValue";

            var hangFireJob = new HangfireJob(testValue);

            var cts = new CancellationTokenSource();

            var jobId = await jobQueueClient.EnqueueAsync<IHangFireTestService>(
                service => service.ExecuteHangfireTestJobAsync(hangFireJob));

            var jobState = await jobQueueClient.GetJobStatusAsync(jobId);
            jobState.Should().NotBeNull().And.Subject.As<JobStatus>().State.Should().Be(JobState.NotStarted);

            cts.CancelAfter(5000);

            Func<Task<JobStatus>> fx = async () => await jobQueueClient.WaitForJobToComplete(jobId, 50 * 1000, cts.Token);
            fx.Should().Throw<TaskCanceledException>();
        }

    }
}
