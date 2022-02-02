// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLoggerProvider.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   Defines the EntityFrameworkLoggerProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

#region Usings

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#endregion

/// <inheritdoc/>
[ProviderAlias("EntityFramework")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API")]
public class EntityFrameworkLoggerProvider<TContext> : EntityFrameworkLoggerProvider<TContext, Log>
    where TContext : DbContext
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to resolve dependency.
    /// </param>
    /// <param name="filter">
    /// The filter used to filter log messages.
    /// </param>
    /// <param name="creator">
    /// The creator used to create new instance of log.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, Log>? creator = null)
        : base(serviceProvider, filter, creator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, IOptions<EntityFrameworkLoggerOptions> options) : base(serviceProvider, options)
    {
    }

    #endregion
}

/// <inheritdoc/>
[ProviderAlias("EntityFramework")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
[SuppressMessage("ReSharper", "MemberCanBeProtected.Global", Justification = "Public API")]
public class EntityFrameworkLoggerProvider<TContext, TLog> : EntityFrameworkLoggerProvider<TContext, TLog, EntityFrameworkLogger<TContext, TLog>>
    where TLog : Log<int>
    where TContext : DbContext
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext,TLog}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to resolve dependency.
    /// </param>
    /// <param name="filter">
    /// The filter used to filter log messages.
    /// </param>
    /// <param name="creator">
    /// The creator used to create new instance of log.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, TLog>? creator = null)
        : base(serviceProvider, filter, creator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext,TLog}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, IOptions<EntityFrameworkLoggerOptions<TLog>> options) : base(serviceProvider, options)
    {
    }

    #endregion
}

/// <inheritdoc/>
[ProviderAlias("EntityFramework")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
[SuppressMessage("ReSharper", "MemberCanBeProtected.Global", Justification = "Public API")]
public class EntityFrameworkLoggerProvider<TContext, TLog, TLogger> : EntityFrameworkLoggerProvider<TContext, TLog, TLogger, int>
    where TLogger : EntityFrameworkLogger<TContext, TLog>
    where TContext : DbContext
    where TLog : Log<int>
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext,TLog,TLogger}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to resolve dependency.
    /// </param>
    /// <param name="filter">
    /// The filter used to filter log messages.
    /// </param>
    /// <param name="creator">
    /// The creator used to create new instance of log.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, TLog>? creator = null)
        : base(serviceProvider, filter, creator)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext,TLog,TLogger}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, IOptions<EntityFrameworkLoggerOptions<TLog>> options) : base(serviceProvider, options)
    {
    }

    #endregion
}

/// <summary>
/// Represents a new instance of an entity framework logger provider for the specified log.
/// </summary>
/// <typeparam name="TContext">
/// The type of the data context class used to access the store.
/// </typeparam>
/// <typeparam name="TLog">
/// The type representing a log.
/// </typeparam>
/// <typeparam name="TLogger">
/// The type of the entity framework logger class used to log.
/// </typeparam>
/// <typeparam name="TKey">
/// The type of the primary key for a log.
/// </typeparam>
[ProviderAlias("EntityFramework")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
[SuppressMessage("ReSharper", "MemberCanBeProtected.Global", Justification = "Public API")]
public class EntityFrameworkLoggerProvider<TContext, TLog, TLogger, TKey> : EntityFrameworkLoggerProviderBase
    where TContext : DbContext
    where TLog : Log<TKey>
    where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
    where TKey : IEquatable<TKey>
{
    #region Fields

    /// <summary>
    /// The function used to create new model instance for a log.
    /// </summary>
    private readonly Func<int, int, string, string, TLog>? creator;

    /// <summary>
    /// The object factory to create new logger used defined types.
    /// </summary>
    private readonly ObjectFactory factory;

    /// <summary>
    /// The function used to filter events based on the log level.
    /// </summary>
    private readonly Func<string, LogLevel, bool> filter;

    /// <summary>
    /// The service provider to resolve dependency.
    /// </summary>
    private readonly IServiceProvider serviceProvider;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext,TLog,TLogger,TKey}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider to resolve dependency.
    /// </param>
    /// <param name="filter">
    /// The filter used to filter log messages.
    /// </param>
    /// <param name="creator">
    /// The creator used to create new instance of log.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, TLog>? creator = null)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        this.creator = creator;
        this.factory = ActivatorUtilities.CreateFactory(
            typeof(TLogger),
            new[] { typeof(string), typeof(Func<string, LogLevel, bool>), typeof(Func<int, int, string, string, TLog>) });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkLoggerProvider{TContext,TLog,TLogger,TKey}"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider.
    /// </param>
    /// <param name="options">
    /// The options.
    /// </param>
    public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, IOptions<EntityFrameworkLoggerOptions<TLog>> options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        // Filter would be applied on LoggerFactory level
        this.filter = TrueFilter;
        this.creator = options.Value.Creator;
        this.factory = ActivatorUtilities.CreateFactory(
            typeof(TLogger),
            new[] { typeof(string), typeof(Func<string, LogLevel, bool>), typeof(Func<int, int, string, string, TLog>) });
    }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public override ILogger CreateLogger(string categoryName)
    {
        this.ThrowIfDisposed();
        return (ILogger)this.factory(this.serviceProvider, new object?[] { categoryName, this.filter, this.creator });
    }

    #endregion
}