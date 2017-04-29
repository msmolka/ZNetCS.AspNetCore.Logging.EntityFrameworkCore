// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLoggerFactoryExtensions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for the <see cref="Microsoft.Extensions.Logging.ILoggerFactory" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// Extension methods for the <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/> class.
    /// </summary>
    public static class EntityFrameworkLoggerFactoryExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>.Information or
        /// higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<int, int, string, string, object[], Log> creator = null)
            where TContext : DbContext
        {
            return AddEntityFramework<TContext>(factory, serviceProvider, LogLevel.Information, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>s of minLevel
        /// or higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="minLevel">
        /// The minimum <see cref="Microsoft.Extensions.Logging.LogLevel"/> to be logged.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            LogLevel minLevel,
            Func<int, int, string, string, object[], Log> creator = null)
            where TContext : DbContext
        {
            return AddEntityFramework<TContext>(factory, serviceProvider, (_, logLevel) => logLevel >= minLevel, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, object[], Log> creator = null)
            where TContext : DbContext
        {
            factory.AddProvider(new EntityFrameworkLoggerProvider<TContext>(serviceProvider, filter, creator));
            return factory;
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>.Information or
        /// higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext, TLog>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<int>
        {
            return AddEntityFramework<TContext, TLog>(factory, serviceProvider, LogLevel.Information, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>s of minLevel
        /// or higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="minLevel">
        /// The minimum <see cref="Microsoft.Extensions.Logging.LogLevel"/> to be logged.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext, TLog>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            LogLevel minLevel,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<int>
        {
            return AddEntityFramework<TContext, TLog>(factory, serviceProvider, (_, logLevel) => logLevel >= minLevel, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext, TLog>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<int>
        {
            factory.AddProvider(new EntityFrameworkLoggerProvider<TContext, TLog>(serviceProvider, filter, creator));
            return factory;
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>.Information or
        /// higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        /// <typeparam name="TLogger">
        /// The type of the entity framework logger class used to log.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext, TLog, TLogger>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<int>
            where TLogger : EntityFrameworkLogger<TContext, TLog>
        {
            return AddEntityFramework<TContext, TLog, TLogger>(factory, serviceProvider, LogLevel.Information, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>s of minLevel
        /// or higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="minLevel">
        /// The minimum <see cref="Microsoft.Extensions.Logging.LogLevel"/> to be logged.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        /// <typeparam name="TLogger">
        /// The type of the entity framework logger class used to log.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext, TLog, TLogger>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            LogLevel minLevel,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<int>
            where TLogger : EntityFrameworkLogger<TContext, TLog>
        {
            return AddEntityFramework<TContext, TLog, TLogger>(factory, serviceProvider, (_, logLevel) => logLevel >= minLevel, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        /// <typeparam name="TLogger">
        /// The type of the entity framework logger class used to log.
        /// </typeparam>
        public static ILoggerFactory AddEntityFramework<TContext, TLog, TLogger>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<int>
            where TLogger : EntityFrameworkLogger<TContext, TLog>
        {
            factory.AddProvider(new EntityFrameworkLoggerProvider<TContext, TLog, TLogger>(serviceProvider, filter, creator));
            return factory;
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>.Information or
        /// higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
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
        public static ILoggerFactory AddEntityFramework<TContext, TLog, TLogger, TKey>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<TKey>
            where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
            where TKey : IEquatable<TKey>
        {
            return AddEntityFramework<TContext, TLog, TLogger, TKey>(factory, serviceProvider, LogLevel.Information, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled for <see cref="Microsoft.Extensions.Logging.LogLevel"/>s of minLevel
        /// or higher.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="minLevel">
        /// The minimum <see cref="Microsoft.Extensions.Logging.LogLevel"/> to be logged.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
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
        public static ILoggerFactory AddEntityFramework<TContext, TLog, TLogger, TKey>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            LogLevel minLevel,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<TKey>
            where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
            where TKey : IEquatable<TKey>
        {
            return AddEntityFramework<TContext, TLog, TLogger, TKey>(factory, serviceProvider, (_, logLevel) => logLevel >= minLevel, creator);
        }

        /// <summary>
        /// Adds an entity framework logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">
        /// The extension method argument.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider for dependency injection.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
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
        public static ILoggerFactory AddEntityFramework<TContext, TLog, TLogger, TKey>(
            this ILoggerFactory factory,
            IServiceProvider serviceProvider,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, object[], TLog> creator = null)
            where TContext : DbContext
            where TLog : Log<TKey>
            where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
            where TKey : IEquatable<TKey>
        {
            factory.AddProvider(new EntityFrameworkLoggerProvider<TContext, TLog, TLogger, TKey>(serviceProvider, filter));
            return factory;
        }

        #endregion
    }
}