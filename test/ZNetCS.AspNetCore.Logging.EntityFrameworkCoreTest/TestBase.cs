// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBase.cs" company="Marcin Smółka zNET Computer Solutions">
//   Copyright (c) Marcin Smółka zNET Computer Solutions. All rights reserved.
// </copyright>
// <summary>
//   The base test class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZNetCS.AspNetCore.Logging.EntityFrameworkCoreTest
{
    #region Usings

    using System.IO;
    using System.Net.Http;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;

    #endregion

    /// <summary>
    /// The base test class.
    /// </summary>
    public abstract class TestBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        /// <param name="version">
        /// The version.
        /// </param>
        protected TestBase(byte version)
        {
            // Arrange
            switch (version)
            {
                case 0:
                    this.Server = new TestServer(
                        new WebHostBuilder()
                            .UseStartup<StartupSimple>()
                            .UseContentRoot(Directory.GetCurrentDirectory()));
                    break;
                case 1:
                    this.Server = new TestServer(
                        new WebHostBuilder()
                            .UseStartup<StartupSimpleException>()
                            .UseContentRoot(Directory.GetCurrentDirectory()));
                    break;
                case 2:
                    this.Server = new TestServer(
                        new WebHostBuilder()
                            .UseStartup<StartupExtended>()
                            .UseContentRoot(Directory.GetCurrentDirectory()));
                    break;
                case 3:
                    this.Server = new TestServer(
                        new WebHostBuilder()
                            .UseStartup<StartupSimpleCreator>()
                            .UseContentRoot(Directory.GetCurrentDirectory()));
                    break;
                case 4:
                    this.Server = new TestServer(
                        new WebHostBuilder()
                            .UseStartup<StartupSimpleNoFilter>()
                            .UseContentRoot(Directory.GetCurrentDirectory()));
                    break;
            }

            this.Client = this.Server.CreateClient();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the client.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public TestServer Server { get; }

        #endregion
    }
}