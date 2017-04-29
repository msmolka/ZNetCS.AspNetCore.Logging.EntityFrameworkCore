﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupSimpleCreator.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest
{
    #region Usings

    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

    #endregion

    /// <summary>
    /// The startup.
    /// </summary>
    public class StartupSimpleCreator
    {
        #region Public Methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The application builder.
        /// </param>
        /// <param name="env">
        /// The hosting environment.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            loggerFactory
                .WithFilter(
                    new FilterLoggerSettings
                    {
                        { "Microsoft", LogLevel.None },
                        { "System", LogLevel.None },
                        { "ZNetCS", LogLevel.Information }
                    })
                .AddEntityFramework<ContextSimple>(
                    serviceProvider,
                    creator: (logLevel, eventId, name, message, args) => new Log
                    {
                        TimeStamp = DateTimeOffset.Now,
                        Level = logLevel,
                        EventId = eventId,
                        Name = "This is my custom log",
                        Message = message
                    });

            ILogger logger = loggerFactory.CreateLogger("Configure");
            app.Use(
                async (context, next) =>
                {
                    logger.LogInformation(1, "Handling request.");
                    await next.Invoke();
                    logger.LogInformation(2, "Finished handling request.");
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
            services.AddEntityFrameworkInMemoryDatabase();

            // Add framework services.
            services.AddDbContext<ContextSimple>(options => options.UseInMemoryDatabase("SimpleLogCreatorDatabase"));
        }

        #endregion
    }
}