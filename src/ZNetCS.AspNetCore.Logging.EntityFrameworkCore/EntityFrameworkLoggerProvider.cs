// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLoggerProvider.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Represents a new instance of an entity framework logger provider for the specified log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// Represents a new instance of an entity framework logger provider for the specified log.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the data context class used to access the store.
    /// </typeparam>
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
        public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, Log> creator = null) : base(serviceProvider, filter, creator)
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
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
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
        public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, TLog> creator = null) : base(serviceProvider, filter, creator)
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
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
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
        public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, TLog> creator = null) : base(serviceProvider, filter, creator)
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
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
    public class EntityFrameworkLoggerProvider<TContext, TLog, TLogger, TKey> : IEntityFrameworkLoggerProvider
        where TContext : DbContext
        where TLog : Log<TKey>
        where TLogger : EntityFrameworkLogger<TContext, TLog, TKey>
        where TKey : IEquatable<TKey>
    {
        #region Fields

        /// <summary>
        /// The function used to create new model instance for a log.
        /// </summary>
        private readonly Func<int, int, string, string, TLog> creator;

        /// <summary>
        /// The function used to filter events based on the log level.
        /// </summary>
        private readonly Func<string, LogLevel, bool> filter;

        /// <summary>
        /// The service provider to resolve dependency.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

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
        public EntityFrameworkLoggerProvider(IServiceProvider serviceProvider, Func<string, LogLevel, bool> filter, Func<int, int, string, string, TLog> creator = null)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.serviceProvider = serviceProvider;
            this.filter = filter;

            if (creator == null)
            {
                this.creator = this.DefaultCreator;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ILoggerProvider

        /// <inheritdoc />
        public virtual ILogger CreateLogger(string categoryName)
        {
            this.ThrowIfDisposed();
            return ActivatorUtilities.CreateInstance<TLogger>(this.serviceProvider, categoryName, this.filter, this.creator);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// True if managed resources should be disposed; otherwise, false.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            this.disposed = true;
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// The default log creator method.
        /// </summary>
        /// <param name="logLevel">
        /// The log level.
        /// </param>
        /// <param name="eventId">
        /// The event id.
        /// </param>
        /// <param name="name">
        /// The log name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        private TLog DefaultCreator(int logLevel, int eventId, string name, string message)
        {
            var log = ActivatorUtilities.CreateInstance<TLog>(this.serviceProvider);

            log.TimeStamp = DateTimeOffset.Now;
            log.Level = logLevel;
            log.EventId = eventId;
            log.Name = name.Length > 255 ? name.Substring(0, 255) : name;
            log.Message = message;

            return log;
        }

        #endregion
    }
}