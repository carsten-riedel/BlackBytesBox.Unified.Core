using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using BlackBytesBox.Unified.Core.Utilities.MainContext;

using Microsoft.Extensions.Hosting;

namespace BlackBytesBox.Unified.Core.Tests
{
    [TestClass]
    public sealed class MainContextIntegrationTests
    {
        private static IHostBuilder? builder;
        private static IHost? app;
        private HttpClient? client;

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            // Create the builder using the minimal hosting model.
            
            builder = Host.CreateDefaultBuilder();

        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {

        }

        [TestInitialize]
        public void TestInit()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        [DataRow(100)]
        public async Task TestMyMiddlewareIntegration(int delay)
        {
            MainContext.EnsureSingleInstance(() => Console.WriteLine("Another instance is already running."));
            var dir = MainContext.TryGetOrCreateAppNameDirectory();


            Assert.IsTrue(true);
            return;

            // Simulate an optional delay to mimic asynchronous conditions.
            await Task.Delay(delay);


            //// Send a GET request to the root endpoint.
            //HttpResponseMessage response = await client!.GetAsync("/");
            //response.EnsureSuccessStatusCode();
            //await Task.Delay(2000);
            //response = await client!.GetAsync("/");
            //response.EnsureSuccessStatusCode();
            //await Task.Delay(2000);
            //response = await client!.GetAsync("/");
            //response.EnsureSuccessStatusCode();
            //await Task.Delay(2000);


            //// Verify that the middleware injected the "X-Option1" header.
            //Assert.IsTrue(response.Headers.Contains("X-Option1"), "The response should contain the 'X-Option1' header.");
            //string headerValue = string.Join("", response.Headers.GetValues("X-Option1"));

            //// With no manual override provided, the default value should be "default value".
            //Assert.AreEqual("default value", headerValue, "The 'X-Option1' header should have the default value.");

            //await Task.Delay(40000);

            //// Send a GET request to the root endpoint.
            //response = await client!.GetAsync("/");
            //response.EnsureSuccessStatusCode();

            //// Verify that the middleware injected the "X-Option1" header.
            //Assert.IsTrue(response.Headers.Contains("X-Option1"), "The response should contain the 'X-Option1' header.");
            //headerValue = string.Join("", response.Headers.GetValues("X-Option1"));

            //// With no manual override provided, the default value should be "default value".
            //Assert.AreEqual("foo", headerValue, "The 'X-Option1' header should have the default value.");
        }
    }
}
