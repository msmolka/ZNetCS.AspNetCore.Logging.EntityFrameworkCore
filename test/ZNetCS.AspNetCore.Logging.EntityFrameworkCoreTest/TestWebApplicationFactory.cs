// -----------------------------------------------------------------------
// <copyright file="TestWebApplicationFactory.cs" company="Marcin Smółka">
// Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest;

#region Usings

using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

#endregion

/// <inheritdoc/>
internal class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    #region Fields

    private readonly TestVersion testVersion;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TestWebApplicationFactory"/> class.
    /// </summary>
    /// <param name="testVersion">Teh testing version.</param>
    public TestWebApplicationFactory(TestVersion testVersion) => this.testVersion = testVersion;

    #endregion

    #region Methods

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(GetPath() ?? string.Empty);
        builder.ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettings.json", false, true); });

        switch (this.testVersion)
        {
            case TestVersion.Simple:

                builder.UseStartup<StartupBuilderSimple>();
                builder.ConfigureLogging(
                    (_, logging) =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("Microsoft", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("System", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("ZNetCS", LogLevel.Information);

                        logging.AddEntityFramework<ContextSimple>();
                    });

                break;
            case TestVersion.SimpleException:

                builder.UseStartup<StartupBuilderSimpleException>();
                builder.ConfigureLogging(
                    (_, logging) =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("Microsoft", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("System", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("ZNetCS", LogLevel.Information);

                        logging.AddEntityFramework<ContextSimple>();
                    });

                break;
            case TestVersion.BuilderExtended:

                builder.UseStartup<StartupBuilderExtended>();
                builder.ConfigureLogging(
                    (_, logging) =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextExtended, ExtendedLog>>("Microsoft", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextExtended, ExtendedLog>>("System", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextExtended, ExtendedLog>>("ZNetCS", LogLevel.Information);

                        logging.AddEntityFramework<ContextExtended, ExtendedLog>();
                    });

                break;
            case TestVersion.BuilderSimpleCreator:

                builder.UseStartup<StartupBuilderSimpleCreator>();
                builder.ConfigureLogging(
                    (_, logging) =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("Microsoft", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("System", LogLevel.None);
                        logging.AddFilter<EntityFrameworkLoggerProvider<ContextSimple>>("ZNetCS", LogLevel.Information);

                        logging.AddEntityFramework<ContextSimple>(
                            opts =>
                            {
                                opts.Creator = (logLevel, eventId, _, message)
                                    => new Log
                                    {
                                        TimeStamp = DateTimeOffset.Now,
                                        Level = logLevel,
                                        EventId = eventId,
                                        Name = "This is my custom log",
                                        Message = message
                                    };
                            });
                    });

                break;
            case TestVersion.BuilderSimpleNoFilter:

                builder.UseStartup<StartupBuilderSimpleNoFilter>();
                builder.ConfigureLogging(
                    (_, logging) =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddEntityFramework<ContextSimple>();
                    });

                break;
            case TestVersion.BuilderSimpleSettings:

                builder.UseStartup<StartupBuilderSimpleSettings>();
                builder.ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddEntityFramework<ContextSimple>();
                    });

                break;
            case TestVersion.BuilderExtendedSettings:

                builder.UseStartup<StartupBuilderExtendedSettings>();
                builder.ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();

                        logging.AddEntityFramework<ContextExtended, ExtendedLog>();
                    });

                break;
        }
    }

    /// <inheritdoc/>
    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder().ConfigureWebHostDefaults(_ => { });

    /// <summary>
    /// Get root path for test web server.
    /// </summary>
    private static string? GetPath()
    {
        string path = Path.GetDirectoryName(typeof(Startup).GetTypeInfo().Assembly.Location)!;

        // ReSharper disable PossibleNullReferenceException
        DirectoryInfo? di = new DirectoryInfo(path).Parent?.Parent?.Parent;

        return di?.FullName;
    }

    #endregion
}