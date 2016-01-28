using System;
using System.Collections.Generic;
using System.Net.Http;
using CostEffectiveCode.Sample.Domain.Entities;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace CostEffectiveCode.WebApi2.Tests.Tests
{
    public class EntityApiControllerTests : IDisposable
    {
        private readonly IDisposable _webApp;
        private readonly HttpClient _httpClient;
        private const string BaseUri = "http://localhost:7777";

        public EntityApiControllerTests()
        {
            _httpClient = new HttpClient();
            _webApp = WebApp.Start<Startup>(BaseUri);
        }

        public void Dispose()
        {
            _webApp.Dispose();
            _httpClient.Dispose();
        }


        [Fact]
        public void AllProductsRequested_AllProductsFetched()
        {
            var message = _httpClient.GetAsync($"{BaseUri}/api/products").Result;
            var json = message.Content.ReadAsStringAsync().Result;

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            // several entities should present in seed!
            Assert.True(products.Count > 1);
        }

        [Fact]
        public void OneProductRequested_OneProductFetched()
        {
            var json = _httpClient.GetAsync($"{BaseUri}/api/products/1").Result.Content.ReadAsStringAsync().Result;

            var product = JsonConvert.DeserializeObject<Product>(json);

            Assert.True(!string.IsNullOrEmpty(product.Name));
        }
    }
}
