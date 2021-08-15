using System;
using RestSharp;

namespace BudTechnicalTests.Framework.GetCountryData
{
    public class GetCountryDataException : Exception
    {
        public GetCountryDataException(string message, IRestResponse? restResponse) : base(message)
        {
            RestResponse = restResponse;
        }

        public IRestResponse? RestResponse { get; }
    }
}
