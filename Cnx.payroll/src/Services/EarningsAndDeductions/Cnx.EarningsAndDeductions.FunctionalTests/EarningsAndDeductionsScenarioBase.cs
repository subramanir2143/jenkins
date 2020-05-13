using Cnx.EarningsAndDeductions.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Cnx.EarningsAndDeductions.FunctionalTests
{
    public class EarningsAndDeductionsScenarioBase
    {
        private const string DownloadApiUrlBase = "api/v1/Download";
        private const string EDApiUrlBase = "api/v1/EarningsAndDeductions";

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(EarningsAndDeductionsScenarioBase)).Location;

            var hostBuilder = new WebHostBuilder()
                    .UseContentRoot(Path.GetDirectoryName(path))
                    .ConfigureAppConfiguration(cb =>
                    {
                        cb.AddJsonFile(
                            "appsettings.json",
                            optional: false)
                        .AddEnvironmentVariables();
                    }).UseStartup<Startup>();

            return new TestServer(hostBuilder);

        }

        public static class Post
        {
            public static string ManualED = $"{EDApiUrlBase}/save";
        }

        public static class Get
        {
            public static string DownloadTemplate = $"{DownloadApiUrlBase}/DownloadTemplate";

            public static string GetRecords = $"{EDApiUrlBase}";
        }
    }
}
