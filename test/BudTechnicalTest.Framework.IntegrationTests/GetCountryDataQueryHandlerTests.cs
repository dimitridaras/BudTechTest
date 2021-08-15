using System.Threading.Tasks;
using BudTechnicalTests.Framework;
using BudTechnicalTests.Framework.GetCountryData;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BudTechnicalTest.Framework.IntegrationTests
{
    public class GetCountryDataQueryHandlerTests
    {
        private IGetCountryDataQueryHandler sut;

        public GetCountryDataQueryHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddFramework();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.sut = serviceProvider.GetRequiredService<IGetCountryDataQueryHandler>();;
        }

        [Fact]
        public async Task GIVEN_valid_iso_code_WHEN_execute_THEN_country_data_returned()
        {
            var expectedTestCountryData = new TestCountryData("Brazil", "Latin America & Caribbean ", "Brasilia", "-47.9292", "-15.7801");

            var countryData = await this.sut.Execute("br");

            countryData.Should().NotBeNull();

            var testCountryData = countryData.Write(new TestCountryDataWriter());

            testCountryData.Should().BeEquivalentTo(expectedTestCountryData);
        }

        [Fact]
        public async Task GIVEN_invalid_iso_code_WHEN_execute_THEN_null_returned()
        {
            var countryData = await this.sut.Execute("bx");

            countryData.Should().BeNull();
        }

        private class TestCountryDataWriter : ICountryDataWriter<TestCountryData>
        {
            public TestCountryData Write(string name, string region, string capital, string longitude, string latitude)
            {
                return new TestCountryData(name, region, capital, longitude, latitude);
            }
        }

        private class TestCountryData
        {
            public TestCountryData(string name, string region, string capital, string longitude, string latitude)
            {
                Name = name;
                Region = region;
                Capital = capital;
                Longitude = longitude;
                Latitude = latitude;
            }

            public string Name { get; }
            public string Region { get; }
            public string Capital { get; }
            public string Longitude { get; }
            public string Latitude { get; }
        }
    }
}
