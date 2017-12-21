// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLoggerBuilderExtendedSettings.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The test logger extended.
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
    /// The test logger extended.
    /// </summary>
    [TestClass]
    public class TestLoggerBuilderExtendedSettings : TestBuilderBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerBuilderExtendedSettings"/> class.
        /// </summary>
        public TestLoggerBuilderExtendedSettings() : base(6)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The write log.
        /// </summary>
        [TestMethod]
        public async Task WriteBuilderExtendedSettingsLog()
        {
            var options = new DbContextOptionsBuilder<ContextExtended>()
                .UseInMemoryDatabase("ExtendedLogDatabase", StartupBuilderExtendedSettings.MemoryRoot)
                .Options;

            // Act
            RequestBuilder request = this.Server.CreateRequest("/");
            HttpResponseMessage response = await request.SendAsync("PUT");

            // Assert
            response.EnsureSuccessStatusCode();

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ContextExtended(options))
            {
                var logs = context.Logs.ToList();

                Assert.AreEqual(2, logs.Count);
                Assert.AreEqual("Handling request.", logs.First().Message);
                Assert.AreEqual(1, logs.First().EventId);
                Assert.AreEqual("Test User", logs.First().User);
                Assert.AreEqual("Test Browser", logs.First().Browser);
                Assert.AreEqual("localhost", logs.First().Host);
                Assert.AreEqual("/", logs.First().Path);
                Assert.AreEqual("Finished handling request.", logs.Last().Message);
                Assert.AreEqual(2, logs.Last().EventId);
            }
        }

        #endregion
    }
}