// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityFrameworkLoggerProvider.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The entity framework logger provider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// The entity framework logger provider interface.
    /// </summary>
    public interface IEntityFrameworkLoggerProvider : ILoggerProvider
    {
    }
}