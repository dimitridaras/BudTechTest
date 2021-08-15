using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BudTechnicalTests.Framework.GetCountryData
{
    internal class GetCountryDataQueryHandler : IGetCountryDataQueryHandler
    {
        private readonly IRestClient restClient;

        public GetCountryDataQueryHandler(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<CountryData?> Execute(string isoCode)
        {
            if (string.IsNullOrWhiteSpace(isoCode)) throw new ArgumentNullException(nameof(isoCode));

            var request = new RestRequest($"country/{isoCode}?format=json", Method.GET, DataFormat.Json);
            var response = await this.restClient.ExecuteAsync(request);

            if (response.Content != null)
            {
                // having to manually deserialize because of the crazy json structure
                var json = JArray.Parse(response.Content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK && json.Count == 2)
                {
                    var countryDataArrayJson = json[1];
                    var countryDataArray = countryDataArrayJson.ToObject<IEnumerable<CountryData>>();
                    return countryDataArray.Single();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                else if (response?.ErrorException != null)
                {
                    throw response.ErrorException;
                }
                else
                {
                    throw new GetCountryDataException("An error occurred retrieving country data", response);
                }
            }

            throw new GetCountryDataException("An error occurred retrieving country data - response has no content", response);
        }
    }
}
