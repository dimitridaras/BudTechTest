namespace BudTechnicalTests.Framework.GetCountryData
{
    public interface ICountryDataWriter<T>
    {
        T Write(string name, string region, string capital, string longitude, string latitude);
    }
}
