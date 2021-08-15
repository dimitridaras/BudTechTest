using System.Threading.Tasks;

namespace BudTechnicalTests.Framework.GetCountryData
{
    public interface IGetCountryDataQueryHandler
    {
        Task<CountryData?> Execute(string isoCode);
    }
}