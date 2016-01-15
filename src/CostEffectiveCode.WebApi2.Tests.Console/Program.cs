using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using CostEffectiveCode.WebApi2.Tests;

namespace CostEffectiveCode.WebApi2.Tests
{
    public class Program
    {
        private const string Uri = "http://localhost:7777/";

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting WebApi Self-Host server");

            using (WebApp.Start<Startup>(Uri))
            {
                var client = new HttpClient();

                var response = client.GetAsync($"{Uri}api/products").Result;

                Console.WriteLine("\n" + response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Console.WriteLine("\n\nSelf-host server is still running. You can debug whatever you want!");
                Console.ReadLine();
            }

        }
    }
}
