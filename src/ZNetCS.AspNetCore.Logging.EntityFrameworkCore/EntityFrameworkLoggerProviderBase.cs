// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLoggerProviderBase.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The entity framework logger provider base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using System;

    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// The entity framework logger provider base.
    /// </summary>
    public abstract class EntityFrameworkLoggerProviderBase : ILoggerProvider
    {
        #region Static Fields

        /// <summary>
        /// The true filter.
        /// </summary>
        protected static readonly Func<string, LogLevel, bool> TrueFilter = (cat, level) => true;

        #endregion

        #region Fields

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public abstract ILogger CreateLogger(string categoryName);

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

        #endregion
    }
}