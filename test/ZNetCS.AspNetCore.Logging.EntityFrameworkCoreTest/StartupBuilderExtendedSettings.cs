// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupBuilderExtendedSettings.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest;

#region Usings

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

/// <summary>
/// The startup.
/// </summary>
public class StartupBuilderExtendedSettings
{
    #region Static Fields

    /// <summary>
    /// The memory root.
    /// </summary>
    public static readonly InMemoryDatabaseRoot MemoryRoot = new InMemoryDatabaseRoot();

    #endregion

    #region Public Methods

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">
    /// The application builder.
    /// </param>
    /// <param name="loggerFactory">
    /// The logger factory.
    /// </param>
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        ILogger logger = loggerFactory.CreateLogger("Configure");
        app.Use(
            async (_, next) =>
            {
                logger.LogInformation(1, "Handling request");
                await next.Invoke();
                logger.LogInformation(2, "Finished handling request");
            });

        app.Run(async context => { await context.Response.WriteAsync("Hello World"); });
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.
        services.AddDbContext<ContextExtended>(options => options.UseInMemoryDatabase("ExtendedLogDatabase", MemoryRoot));

        // requires for http context access.
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    #endregion
}