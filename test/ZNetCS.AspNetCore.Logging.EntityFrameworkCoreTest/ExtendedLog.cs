// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedLog.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The extended log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest
{
    #region Usings

    using Microsoft.AspNetCore.Http;

    using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

    #endregion

    /// <summary>
    /// The extended log.
    /// </summary>
    public class ExtendedLog : Log
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLog"/> class.
        /// </summary>
        /// <param name="accessor">
        /// The accessor.
        /// </param>
        public ExtendedLog(IHttpContextAccessor accessor)
        {          
            this.Browser = "Test Browser";
            this.Host = "localhost";
            this.User = "Test User";
            this.Path = accessor.HttpContext.Request.Path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLog"/> class.
        /// </summary>
        protected ExtendedLog()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the browser.
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public string User { get; set; }

        #endregion
    }
}