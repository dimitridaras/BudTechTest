using System.Runtime.CompilerServices;
using BudTechnicalTests.Framework.GetCountryData;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

[assembly: InternalsVisibleTo("BudTechnicalTest.Framework.IntegrationTests"),
           InternalsVisibleTo("BudTechnicalTest.Framework.UnitTests"),
           InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace BudTechnicalTests.Framework
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFramework(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRestClient>((serviceProvider) => new RestClient("http://api.worldbank.org/v2"));
            serviceCollection.AddTransient<IGetCountryDataQueryHandler, GetCountryDataQueryHandler>();
        }
    }
}
