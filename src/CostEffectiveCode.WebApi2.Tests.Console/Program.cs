using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace CostEffectiveCode.WebApi2.Tests.Console
{
    public class Program
    {
        private const string BaseUri = "http://localhost:7777/";

        public static void Main(string[] args)
        {
            System.Console.WriteLine("Starting WebApi Self-Host server");

            using (WebApp.Start<Startup>(BaseUri))
            {
                var client = new HttpClient();

                var response = client.GetAsync($"{BaseUri}api/products/1").Result;

                System.Console.WriteLine("\n" + response);
                System.Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                System.Console.WriteLine("\n\nSelf-host server is still running. You can debug whatever you want!");
                System.Console.ReadLine();
            }

        }
    }
}
