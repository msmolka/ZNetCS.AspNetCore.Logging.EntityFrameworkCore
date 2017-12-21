// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLoggerBuilderSimpleException.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The test logger simple exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest
{
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
    public class TestLoggerBuilderSimpleException : TestBuilderBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerBuilderSimpleException"/> class.
        /// </summary>
        public TestLoggerBuilderSimpleException() : base(1)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The write log.
        /// </summary>
        [TestMethod]
        public async Task WriteBuilderSimpleLogException()
        {
            var options = new DbContextOptionsBuilder<ContextSimple>()
                .UseInMemoryDatabase("SimpleLogExceptionDatabase", StartupBuilderSimpleException.MemoryRoot)
                .Options;

            // Act
            RequestBuilder request = this.Server.CreateRequest("/");
            HttpResponseMessage response = await request.SendAsync("PUT");

            // Assert
            response.EnsureSuccessStatusCode();

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ContextSimple(options))
            {
                var logs = context.Logs.ToList();

                Assert.AreEqual(1, logs.Count);
                Assert.AreEqual(true, logs.First().Message.StartsWith("Exception message"));
                Assert.AreEqual(1, logs.First().EventId);
            }
        }

        #endregion
    }
}