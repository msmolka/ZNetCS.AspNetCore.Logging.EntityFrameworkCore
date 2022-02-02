// -----------------------------------------------------------------------
// <copyright file="Log.cs" company="Marcin Smółka">
// Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

#region Usings

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

/// <summary>
/// The default implementation of <see cref="Log{TKey}"/> which uses a integer as a primary key.
/// </summary>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API")]
public class Log : Log<int>
{
}

/// <summary>
/// Represents a log in the logging database.
/// </summary>
/// <typeparam name="TKey">
/// The type used for the primary key for the log.
/// </typeparam>
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "OK")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Public API")]
public class Log<TKey> where TKey : IEquatable<TKey>
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    public virtual int EventId { get; set; }

    /// <summary>
    /// Gets or sets the primary key for this log.
    /// </summary>
    public virtual TKey Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the level.
    /// </summary>
    public virtual int Level { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public virtual string Message { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public virtual string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    public virtual DateTimeOffset TimeStamp { get; set; }

    #endregion
}