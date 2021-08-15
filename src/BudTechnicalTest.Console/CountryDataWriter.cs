using BudTechnicalTests.Framework.GetCountryData;
using static System.Environment;

namespace BudTechnicalTest.Console
{
    public class CountryDataWriter : ICountryDataWriter<string>
    {
        public string Write(string name, string region, string capital, string longitude, string latitude)
        {
            return $"Country name: {name}{NewLine}Region: {region}{NewLine}Capital: {capital}{NewLine}Longitude: {longitude}{NewLine}Latitude: {latitude}";
        }
    }
}
