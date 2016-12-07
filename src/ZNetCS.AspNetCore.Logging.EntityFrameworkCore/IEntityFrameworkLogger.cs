// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityFrameworkLogger.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The EntityFrameworkLogger interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// The EntityFrameworkLogger interface.
    /// </summary>
    public interface IEntityFrameworkLogger : ILogger
    {
    }
}