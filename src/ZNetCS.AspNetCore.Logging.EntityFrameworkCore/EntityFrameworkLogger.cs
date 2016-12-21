// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLogger.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   Represents a new instance of an entity framework logger for the specified log.
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
    /// Represents a new instance of an entity framework logger for the specified log.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the data context class used to access the store.
    /// </typeparam>
    public class EntityFrameworkLogger<TContext> : EntityFrameworkLogger<TContext, Log>
        where TContext : DbContext
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkLogger{TContext}"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider to resolve dependency.
        /// </param>
        /// <param name="name">
        /// The name of the logger.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        public EntityFrameworkLogger(
            IServiceProvider serviceProvider,
            string name,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, Log> creator = null)
            : base(serviceProvider, name, filter, creator)
        {
        }

        #endregion
    }

    /// <summary>
    /// Represents a new instance of an entity framework logger for the specified log.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the data context class used to access the store.
    /// </typeparam>
    /// <typeparam name="TLog">
    /// The type representing a log.
    /// </typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
    public class EntityFrameworkLogger<TContext, TLog> : EntityFrameworkLogger<TContext, TLog, int>
        where TContext : DbContext
        where TLog : Log<int>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkLogger{TContext,TLog}"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider to resolve dependency.
        /// </param>
        /// <param name="name">
        /// The name of the logger.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        public EntityFrameworkLogger(
            IServiceProvider serviceProvider,
            string name,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, TLog> creator = null)
            : base(serviceProvider, name, filter, creator)
        {
        }

        #endregion
    }

    /// <summary>
    /// Represents a new instance of an entity framework logger for the specified log.
    /// </summary>
    /// <typeparam name="TContext">
    /// The type of the data context class used to access the store.
    /// </typeparam>
    /// <typeparam name="TLog">
    /// The type representing a log.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the primary key for a log.
    /// </typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
    public class EntityFrameworkLogger<TContext, TLog, TKey> : IEntityFrameworkLogger
        where TContext : DbContext
        where TLog : Log<TKey>
        where TKey : IEquatable<TKey>
    {
        #region Fields

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
        /// Initializes a new instance of the <see cref="EntityFrameworkLogger{TContext,TLog,TKey}"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider to resolve dependency.
        /// </param>
        /// <param name="name">
        /// The name of the logger.
        /// </param>
        /// <param name="filter">
        /// The function used to filter events based on the log level.
        /// </param>
        /// <param name="creator">
        /// The creator used to create new instance of log.
        /// </param>
        public EntityFrameworkLogger(
            IServiceProvider serviceProvider,
            string name,
            Func<string, LogLevel, bool> filter,
            Func<int, int, string, string, TLog> creator = null)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            this.serviceProvider = serviceProvider;

            this.filter = filter;

            this.Name = name ?? string.Empty;
            this.Creator = creator ?? this.DefaultCreator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the function used to create new model instance for a log.
        /// </summary>
        protected virtual Func<int, int, string, string, TLog> Creator { get; }

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        protected virtual string Name { get; }

        #endregion

        #region Implemented Interfaces

        #region ILogger

        /// <inheritdoc />
        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        /// <inheritdoc />
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return (this.filter == null) || this.filter(this.Name, logLevel);
        }

        /// <inheritdoc />
        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{message}";

            if (exception != null)
            {
                message += $"{Environment.NewLine}{Environment.NewLine}{exception}";
            }

            this.WriteMessage(message, logLevel, eventId.Id);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Writes message to database.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        /// <param name="logLevel">
        /// The log level to write.
        /// </param>
        /// <param name="eventId">
        /// The event id to write.
        /// </param>
        protected virtual void WriteMessage(string message, LogLevel logLevel, int eventId)
        {
            // create separate context for adding log
            using (var context = ActivatorUtilities.CreateInstance<TContext>(this.serviceProvider))
            {
                // create new log with resolving dependency injection
                TLog log = this.Creator((int)logLevel, eventId, this.Name, message);

                context.Set<TLog>().Add(log);

                context.SaveChanges();
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
        /// <param name="logName">
        /// The log name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        private TLog DefaultCreator(int logLevel, int eventId, string logName, string message)
        {
            var log = ActivatorUtilities.CreateInstance<TLog>(this.serviceProvider);

            log.TimeStamp = DateTimeOffset.Now;
            log.Level = logLevel;
            log.EventId = eventId;
            log.Name = logName.Length > 255 ? logName.Substring(0, 255) : logName;
            log.Message = message;

            return log;
        }

        #endregion

        #region Nested Classes

        #region NoopDisposable

        /// <summary>
        /// The noop disposable.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "OK")]
        private class NoopDisposable : IDisposable
        {
            #region Static Fields

            /// <summary>
            /// The instance.
            /// </summary>
            public static readonly NoopDisposable Instance = new NoopDisposable();

            #endregion

            #region Implemented Interfaces

            #region IDisposable

            /// <summary>
            /// The dispose.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion

            #endregion
        }

        #endregion

        #endregion
    }
}