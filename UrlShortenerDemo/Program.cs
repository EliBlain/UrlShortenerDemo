using BitlyAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace UrlShortenerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start!
            MainAsync(args).Wait();
        }

        //just so I can read in appsettings and ignore them in .gitignore
        static async Task MainAsync(string[] args)
        {
            // Create service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                await serviceProvider.GetService<ShortenerDemo>().Run();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add access to generic IConfigurationRoot
            serviceCollection.AddTransient<ShortenerDemo>();
        }
    }        
}
