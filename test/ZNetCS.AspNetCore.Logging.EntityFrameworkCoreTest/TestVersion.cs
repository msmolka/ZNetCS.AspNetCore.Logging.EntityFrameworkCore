// -----------------------------------------------------------------------
// <copyright file="TestVersion.cs" company="Marcin Smółka">
// Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest;

/// <summary>
/// The test version logging.
/// </summary>
internal enum TestVersion
{
    /// <summary>
    /// Simple logging.
    /// </summary>
    Simple = 0,

    /// <summary>
    /// Simple logging with exception.
    /// </summary>
    SimpleException = 1,

    /// <summary>
    /// Builder logging.
    /// </summary>
    BuilderExtended = 2,

    /// <summary>
    /// Builder logging with creator.
    /// </summary>
    BuilderSimpleCreator = 3,

    /// <summary>
    /// Builder simple logging with no filer.
    /// </summary>
    BuilderSimpleNoFilter = 4,

    /// <summary>
    /// Builder simple logging using settings.
    /// </summary>
    BuilderSimpleSettings = 5,

    /// <summary>
    /// Builder extended logging using settings.
    /// </summary>
    BuilderExtendedSettings = 6
}