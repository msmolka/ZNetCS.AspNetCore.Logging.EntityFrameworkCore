// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLoggerBuilderExtensions.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The entity framework logger builder extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// The entity framework logger builder extensions.
    /// </summary>
    public static class EntityFrameworkLoggerBuilderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        public static ILoggingBuilder AddEntityFramework<TContext>(this ILoggingBuilder builder) where TContext : DbContext
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, EntityFrameworkLoggerProvider<TContext>>());

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
        /// </param>
        /// <param name="configure">
        /// The <see cref="ILoggingBuilder"/> configuration delegate.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        public static ILoggingBuilder AddEntityFramework<TContext>(this ILoggingBuilder builder, Action<EntityFrameworkLoggerOptions> configure)
            where TContext : DbContext
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddEntityFramework<TContext>();
            builder.Services.Configure(configure);

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        public static ILoggingBuilder AddEntityFramework<TContext, TLog>(this ILoggingBuilder builder)
            where TContext : DbContext
            where TLog : Log<int>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, EntityFrameworkLoggerProvider<TContext, TLog>>());

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
        /// </param>
        /// <param name="configure">
        /// The <see cref="ILoggingBuilder"/> configuration delegate.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the data context class used to access the store.
        /// </typeparam>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        public static ILoggingBuilder AddEntityFramework<TContext, TLog>(this ILoggingBuilder builder, Action<EntityFrameworkLoggerOptions<TLog>> configure)
            where TContext : DbContext
            where TLog : Log<int>
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddEntityFramework<TContext, TLog>();
            builder.Services.Configure(configure);

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
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
        public static ILoggingBuilder AddEntityFramework<TContext, TLog, TLogger>(this ILoggingBuilder builder)
            where TContext : DbContext
            where TLog : Log<int>
            where TLogger : EntityFrameworkLogger<TContext, TLog>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, EntityFrameworkLoggerProvider<TContext, TLog, TLogger>>());

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
        /// </param>
        /// <param name="configure">
        /// The <see cref="ILoggingBuilder"/> configuration delegate.
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
        public static ILoggingBuilder AddEntityFramework<TContext, TLog, TLogger>(this ILoggingBuilder builder, Action<EntityFrameworkLoggerOptions<TLog>> configure)
            where TContext : DbContext
            where TLog : Log<int>
            where TLogger : EntityFrameworkLogger<TContext, TLog>
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddEntityFramework<TContext, TLog, TLogger>();
            builder.Services.Configure(configure);

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
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
        public static ILoggingBuilder AddEntityFramework<TContext, TLog, TLogger, TKey>(this ILoggingBuilder builder)
            where TContext : DbContext
            where TLog : Log<TKey>
            where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
            where TKey : IEquatable<TKey>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, EntityFrameworkLoggerProvider<TContext, TLog, TLogger, TKey>>());

            return builder;
        }

        /// <summary>
        /// Adds a entity framework logger named 'EntityFramework' to the factory.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="ILoggingBuilder"/> to use.
        /// </param>
        /// <param name="configure">
        /// The <see cref="ILoggingBuilder"/> configuration delegate.
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
        public static ILoggingBuilder AddEntityFramework<TContext, TLog, TLogger, TKey>(
            this ILoggingBuilder builder,
            Action<EntityFrameworkLoggerOptions<TLog>> configure)
            where TContext : DbContext
            where TLog : Log<TKey>
            where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
            where TKey : IEquatable<TKey>
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddEntityFramework<TContext, TLog, TLogger, TKey>();
            builder.Services.Configure(configure);

            return builder;
        }

        #endregion
    }
}