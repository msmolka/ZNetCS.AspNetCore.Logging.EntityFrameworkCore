// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLoggerBuilderSimpleException.cs" company="Marcin Smółka">
//   Copyright (c) Marcin Smółka. All rights reserved.
// </copyright>
// <summary>
//   The test logger simple exception.
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
/// The test logger simple exception.
/// </summary>
[TestClass]
public class TestLoggerBuilderSimpleException
{
    #region Public Methods

    /// <summary>
    /// The write log.
    /// </summary>
    [TestMethod]
    public async Task WriteBuilderSimpleLogException()
    {
        using var factory = new TestWebApplicationFactory(TestVersion.SimpleException);

        var options = new DbContextOptionsBuilder<ContextSimple>()
            .UseInMemoryDatabase("SimpleLogExceptionDatabase", StartupBuilderSimpleException.MemoryRoot)
            .Options;

        // Act
        RequestBuilder request = factory.Server.CreateRequest("/");
        HttpResponseMessage response = await request.SendAsync("PUT");

        // Assert
        response.EnsureSuccessStatusCode();

        // Use a separate instance of the context to verify correct data was saved to database
        await using var context = new ContextSimple(options);
        var logs = context.Logs.ToList();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual(true, logs.First().Message.StartsWith("Exception message"));
        Assert.AreEqual(1, logs.First().EventId);
    }

    #endregion
}