// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupSimpleException.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#if NETCOREAPP2_0
namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest
{
    #region Usings

    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

    #endregion

    /// <summary>
    /// The startup.
    /// </summary>
    public class StartupSimpleException
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
                .AddEntityFramework<ContextSimple>(serviceProvider);

            ILogger logger = loggerFactory.CreateLogger("Configure");

            app.Use(
                async (context, next) =>
                {
                    try
                    {
                        throw new Exception("By purpose");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(1, ex, "Exception message");
                    }

                    await next.Invoke();
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
            services.AddDbContext<ContextSimple>(options => options.UseInMemoryDatabase("SimpleLogExceptionDatabase", MemoryRoot));
        }

        #endregion
    }
}
#endif