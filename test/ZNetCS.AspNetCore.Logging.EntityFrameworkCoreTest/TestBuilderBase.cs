// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBuilderBase.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The test builder base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest
{
    #region Usings

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

    #endregion

    /// <summary>
    /// The test builder base.
    /// </summary>
    public class TestBuilderBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBuilderBase"/> class.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        protected TestBuilderBase(byte version)
        {
            // Arrange
            switch (version)
            {
                case 0:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderSimple>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("Microsoft", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("System", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("ZNetCS", LogLevel.Information);

                                logging.AddEntityFramework<ContextSimple>();
                            }));

                    break;
                case 1:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderSimpleException>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("Microsoft", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("System", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("ZNetCS", LogLevel.Information);

                                logging.AddEntityFramework<ContextSimple>();
                            }));

                    break;
                case 2:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderExtended>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextExtended, ExtendedLog>>("Microsoft", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextExtended, ExtendedLog>>("System", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextExtended, ExtendedLog>>("ZNetCS", LogLevel.Information);

                                logging.AddEntityFramework<ContextExtended, ExtendedLog>();
                            }));
                    break;
                case 3:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderSimpleCreator>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("Microsoft", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("System", LogLevel.None);
                                logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("ZNetCS", LogLevel.Information);

                                logging.AddEntityFramework<ContextSimple>(
                                    opts =>
                                    {
                                        opts.Creator = (logLevel, eventId, name, message) => new Log
                                        {
                                            TimeStamp = DateTimeOffset.Now,
                                            Level = logLevel,
                                            EventId = eventId,
                                            Name = "This is my custom log",
                                            Message = message
                                        };
                                    });
                            }));

                    break;
                case 4:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderSimpleNoFilter>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddEntityFramework<ContextSimple>();
                            }));
                    break;

                case 5:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderSimpleSettings>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddEntityFramework<ContextSimple>();
                            }));
                    break;

                case 6:
                    this.Server = new TestServer(
                        this.CreateBuilder<StartupBuilderExtendedSettings>().ConfigureLogging(
                            (hostingContext, logging) =>
                            {
                                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                logging.AddConsole();
                                logging.AddDebug();

                                logging.AddEntityFramework<ContextExtended, ExtendedLog>();
                            }));
                    break;
            }

            this.Client = this.Server.CreateClient();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the client.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public TestServer Server { get; }

        #endregion

        /// <summary>
        /// The create builder.
        /// </summary>
        /// <typeparam name="TStartup">
        /// The startup type.
        /// </typeparam>
        private IWebHostBuilder CreateBuilder<TStartup>() where TStartup : class
        {
            var path = Path.GetDirectoryName(typeof(StartupSimple).GetTypeInfo().Assembly.Location);

            // ReSharper disable PossibleNullReferenceException
            var di = new DirectoryInfo(path).Parent.Parent.Parent;

            return new WebHostBuilder()
                .UseStartup<TStartup>()
                .UseContentRoot(di.FullName)
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettings.json", true, true); });

            // ReSharper enable PossibleNullReferenceException
        }
    }
}