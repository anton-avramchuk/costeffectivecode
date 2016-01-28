using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Xunit;

namespace CostEffectiveCode.WebApi2.Tests.Tests
{
    public class EntityApiControllerTests : IDisposable
    {
        private readonly IDisposable _webApp;
        private const string Uri = "http://localhost:7777/";

        public EntityApiControllerTests()
        {
            _webApp = WebApp.Start<Startup>(Uri);
        }

        public void Dispose()
        {
            _webApp.Dispose();
        }


        [Fact]
        public void TestConsole_Test()
        {
            var client = new HttpClient();

            var response = client.GetAsync($"{Uri}products").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

    }
}
