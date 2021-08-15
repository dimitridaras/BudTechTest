using System;
using System.Threading.Tasks;
using BudTechnicalTests.Framework;
using BudTechnicalTests.Framework.GetCountryData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BudTechnicalTest.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                System.Console.WriteLine("Usage:  BudTechnicalTest.Console {isoCode}");
                return;
            }

            using IHost host = CreateHostBuilder(args).Build();

            await GetCountryData(host.Services, args[0]);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddFramework());

        static async Task GetCountryData(IServiceProvider serviceProvider, string isoCode)
        {
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var getCountryDataQueryHandler = provider.GetRequiredService<IGetCountryDataQueryHandler>();

            var countryData = await getCountryDataQueryHandler.Execute(isoCode);
            if (countryData != null)
            {
                System.Console.WriteLine(countryData.Write(new CountryDataWriter()));
            }
            else
            {
                System.Console.WriteLine("Invalid iso code");
            }
        }
    }
}
