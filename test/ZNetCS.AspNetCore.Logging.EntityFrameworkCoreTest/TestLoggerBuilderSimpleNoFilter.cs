// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLoggerBuilderSimpleNoFilter.cs" company="Marcin Smółka">
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
public class TestLoggerBuilderSimpleNoFilter
{
    #region Public Methods

    /// <summary>
    /// The write log.
    /// </summary>
    [TestMethod]
    public async Task WriteBuilderSimpleNoFilterLog()
    {
        using var factory = new TestWebApplicationFactory(TestVersion.BuilderSimpleNoFilter);

        var options = new DbContextOptionsBuilder<ContextSimple>()
            .UseInMemoryDatabase("SimpleLogNoFilterDatabase", StartupBuilderSimpleNoFilter.MemoryRoot)
            .Options;

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();

        // Use a separate instance of the context to verify correct data was saved to database
        await using var context = new ContextSimple(options);
        var logs = context.Logs.ToList();

        Assert.AreEqual("Handling request", logs[0].Message);
        Assert.AreEqual(1, logs[0].EventId);
        Assert.AreEqual("Finished handling request", logs[1].Message);
        Assert.AreEqual(2, logs[1].EventId);
    }

    #endregion
}