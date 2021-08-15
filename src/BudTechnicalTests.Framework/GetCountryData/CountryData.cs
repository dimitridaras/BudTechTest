namespace BudTechnicalTests.Framework.GetCountryData
{
    public class CountryData
    {
        public string Name { private get; set; } = null!;
        public Region Region { private get; set; } = null!;
        public string CapitalCity { private get; set; } = null!;
        public string Longitude { private get; set; } = null!;
        public string Latitude { private get; set; } = null!;

        public T Write<T>(ICountryDataWriter<T> countryDataWriter)
        {
            return countryDataWriter.Write(Name, Region.Value, CapitalCity, Longitude, Latitude);
        }
    }
}
