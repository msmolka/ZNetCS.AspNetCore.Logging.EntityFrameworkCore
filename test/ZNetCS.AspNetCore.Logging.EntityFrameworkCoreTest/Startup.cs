// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Marcin Smółka">
// Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest;

#region Usings

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

/// <summary>
/// The startup.
/// </summary>
public class Startup
{
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
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    public void ConfigureServices(IServiceCollection services)
    {
    }

    #endregion
}