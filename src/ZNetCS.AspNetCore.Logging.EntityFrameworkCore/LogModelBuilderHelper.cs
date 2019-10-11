// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogModelBuilderHelper.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The model builder helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCore
{
    #region Usings

    using System;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #endregion

    /// <summary>
    /// The model builder helper.
    /// </summary>
    public static class LogModelBuilderHelper
    {
        #region Public Methods

        /// <summary>
        /// The build helper method.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public static void Build(EntityTypeBuilder<Log> builder)
        {
            Build<Log>(builder);
        }

        /// <summary>
        /// The build helper method.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        public static void Build<TLog>(EntityTypeBuilder<TLog> builder)
            where TLog : Log<int>
        {
            Build<TLog, int>(builder);
        }

        /// <summary>
        /// The build helper method.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <typeparam name="TLog">
        /// The type representing a log.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the primary key for a log.
        /// </typeparam>
        public static void Build<TLog, TKey>(EntityTypeBuilder<TLog> builder)
            where TLog : Log<TKey>
            where TKey : IEquatable<TKey>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name).HasMaxLength(255);
        }

        #endregion
    }
}