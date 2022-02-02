// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSimple.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest;

#region Usings

using Microsoft.EntityFrameworkCore;

using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

#endregion

/// <summary>
/// The context.
/// </summary>
public class ContextSimple : DbContext
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextSimple"/> class.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    public ContextSimple(DbContextOptions options) : base(options)
    {
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the logs.
    /// </summary>
    public DbSet<Log> Logs => this.Set<Log>();

    #endregion

    #region Methods

    /// <summary>
    /// The on model creating.
    /// </summary>
    /// <param name="modelBuilder">
    /// The model builder.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // build default model.
        LogModelBuilderHelper.Build(modelBuilder.Entity<Log>());

        // real relation database can map table:
        // modelBuilder.Entity<Log>().ToTable("Log");
    }

    #endregion
}