// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLoggerBuilderSimpleSettings.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The test logger simple.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest;

#region Usings

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

/// <summary>
/// The test logger simple.
/// </summary>
[TestClass]
public class TestLoggerBuilderSimpleSettings
{
    #region Public Methods

    /// <summary>
    /// The write log.
    /// </summary>
    [TestMethod]
    public async Task WriteBuilderSimpleSettingsLog()
    {
        using var factory = new TestWebApplicationFactory(TestVersion.BuilderSimpleSettings);

        var options = new DbContextOptionsBuilder<ContextSimple>()
            .UseInMemoryDatabase("SimpleLogDatabase", StartupBuilderSimpleSettings.MemoryRoot)
            .Options;

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();

        // Use a separate instance of the context to verify correct data was saved to database
        await using var context = new ContextSimple(options);
        var logs = context.Logs.ToList();

        Assert.AreEqual(2, logs.Count);
        Assert.AreEqual("Handling request", logs.First().Message);
        Assert.AreEqual(1, logs.First().EventId);
        Assert.AreEqual("Finished handling request", logs.Last().Message);
        Assert.AreEqual(2, logs.Last().EventId);
    }

    #endregion
}