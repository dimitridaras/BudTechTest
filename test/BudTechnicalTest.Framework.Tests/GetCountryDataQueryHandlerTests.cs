using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BudTechnicalTests.Framework.GetCountryData;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace BudTechnicalTest.Framework.Tests
{
    public class GetCountryDataQueryHandlerTests
    {
        private GetCountryDataQueryHandler sut;
        private Mock<IRestClient> mockRestClient;

        public GetCountryDataQueryHandlerTests()
        {
            this.mockRestClient = new Mock<IRestClient>();
            this.sut = new GetCountryDataQueryHandler(this.mockRestClient.Object);
        }

        [Fact]
        public async Task GIVEN_iso_code_supplied_WHEN_execute_THEN_api_called_with_iso_code()
        {
            var json = GetFromResources("CountryData");

            var response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);
            response.Setup(_ => _.Content).Returns(json);

            this.mockRestClient
                .Setup(x => x.ExecuteAsync(It.Is<RestRequest>(rr => rr.Method == Method.GET && rr.RequestFormat == DataFormat.Json), default(CancellationToken)))
                .ReturnsAsync(response.Object)
                .Verifiable();

            var countryData = await this.sut.Execute("foo");
            this.mockRestClient.Verify();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GIVEN_iso_code_not_supplied_WHEN_execute_THEN_exception_throw(string isoCode)
        {
            Func<Task> func = async () => await this.sut.Execute(isoCode);

            func.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GIVEN_api_returns_country_data_WHEN_execute_THEN_country_data_returned()
        {
            var expectedTestCountryData = new TestCountryData("Brazil", "Latin America & Caribbean ", "Brasilia", "-47.9292", "-15.7801");

            var json = GetFromResources("CountryData");

            var response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);
            response.Setup(_ => _.Content).Returns(json);

            this.mockRestClient
                .Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken)))
                .ReturnsAsync(response.Object);

            var countryData = await this.sut.Execute("foo");

            countryData.Should().NotBeNull();

            var testCountryData = countryData.Write(new TestCountryDataWriter());

            testCountryData.Should().BeEquivalentTo(expectedTestCountryData);
        }

        [Fact]
        public async Task GIVEN_api_returns_invalid_iso_code_WHEN_execute_THEN_null_returned()
        {
            var json = GetFromResources("ErrorData");

            var response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode).Returns(HttpStatusCode.OK);
            response.Setup(_ => _.Content).Returns(json);

            this.mockRestClient
                .Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken)))
                .ReturnsAsync(response.Object);

            var countryData = await this.sut.Execute("foo");

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

        private IRestClient MockRestClient(HttpStatusCode httpStatusCode, string json)
        {
            var response = new Mock<IRestResponse>();
            response.Setup(_ => _.StatusCode).Returns(httpStatusCode);
            response.Setup(_ => _.Content).Returns(json);

            this.mockRestClient
              .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), default(CancellationToken)))
              .ReturnsAsync(response.Object);
            return mockRestClient.Object;
        }

        internal string GetFromResources(string resourceName)
        {
            var assembly = this.GetType().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}