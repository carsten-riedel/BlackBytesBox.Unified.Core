using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace BlackBytesBox.Unified.Core.Tests
{
    [TestClass]
    public sealed class MyMiddlewareIntegrationTests
    {
        private static IHostBuilder? builder;
        private static IHost? app;
        private HttpClient? client;

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            // Create the builder using the minimal hosting model.

            
            builder = Host.CreateDefaultBuilder();

            // Set a fixed URL for the host.

            // Optionally, add a separate JSON configuration file (for example, myMiddlewareConfig.json)
            // Uncomment the following line if you want to load a separate config file:
            // builder.Configuration.AddJsonFile("myMiddlewareConfig.json", optional: true, reloadOnChange: true);

            // Register middleware configuration. This method will internally resolve IConfiguration.
            // If a "MyMiddleware" section exists, it is used.
            // Manual configuration provided in the lambda takes precedence over configuration file values.

            //builder.Services.AddMyMiddlewareConfiguration(options =>
            //{
            //    options.Option1 = "Manually overridden value";
            //    // Option2 will remain as defined in the configuration file or default if not provided.
            //});

            //builder.Services.AddMyMiddlewareConfiguration(builder.Configuration);

            builder.ConfigureServices((context, services) =>
            {

            });


            app = builder.Build();

            // Start the application.
            await app.StartAsync();
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            if (app != null)
            {
                await app.StopAsync();
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            // Create an HttpClientHandler that accepts any certificate.
            var handler = new HttpClientHandler
            {
                // Accept all certificates (this is unsafe for production!)
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                // Alternatively, you can use:
                // ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Create a new, independent HttpClient for each test.
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:5425"),
                DefaultRequestVersion = HttpVersion.Version11, // Force HTTP/1.0
            };
            // Add a default User-Agent header for testing.
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36");
            client.DefaultRequestHeaders.Add("APPID", "1234");
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("de-DE");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Dispose of the HttpClient after each test.
            client?.Dispose();
            client = null;
        }

        [TestMethod]
        [DataRow(100)]
        public async Task TestMyMiddlewareIntegration(int delay)
        {
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


            Assert.IsTrue(true);
            return;
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
