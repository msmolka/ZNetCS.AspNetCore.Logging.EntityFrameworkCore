// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkLoggerOptions.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The entity framework logger options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

    #region Usings

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

/// <summary>
/// The entity framework logger options.
/// </summary>
public class EntityFrameworkLoggerOptions : EntityFrameworkLoggerOptions<Log>
{
}

/// <summary>
/// The entity framework logger options.
/// </summary>
/// <typeparam name="TLog">
/// The log model type.
/// </typeparam>
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Public API")]
public class EntityFrameworkLoggerOptions<TLog>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the creator.
    /// </summary>
    public Func<int, int, string, string, TLog>? Creator { get; set; }

    #endregion
}