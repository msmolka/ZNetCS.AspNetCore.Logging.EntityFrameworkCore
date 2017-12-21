// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLoggerBuilderSimpleCreator.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The test logger simple.
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
    /// The test logger simple.
    /// </summary>
    [TestClass]
    public class TestLoggerBuilderSimpleCreator : TestBuilderBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerBuilderSimpleCreator"/> class.
        /// </summary>
        public TestLoggerBuilderSimpleCreator() : base(3)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The write log.
        /// </summary>
        [TestMethod]
        public async Task WriteBuilderSimpleCreatorLog()
        {
            var options = new DbContextOptionsBuilder<ContextSimple>()
                .UseInMemoryDatabase("SimpleLogCreatorDatabase", StartupBuilderSimpleCreator.MemoryRoot)
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

                Assert.AreEqual(2, logs.Count);
                Assert.AreEqual("Handling request Test2.", logs.First().Message);
                Assert.AreEqual("This is my custom log", logs.First().Name);
                Assert.AreEqual(1, logs.First().EventId);
                Assert.AreEqual("Finished handling request param 1.", logs.Last().Message);
                Assert.AreEqual(2, logs.Last().EventId);
            }
        }

        #endregion
    }
}