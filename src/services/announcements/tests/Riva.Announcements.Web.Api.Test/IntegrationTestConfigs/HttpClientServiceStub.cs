using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
{
    public class HttpClientServiceStub : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public Uri BaseAddress
        {
            get => _httpClient.BaseAddress;
            set => _httpClient.BaseAddress = value;
        }

        public HttpRequestHeaders DefaultRequestHeaders => _httpClient.DefaultRequestHeaders;

        public HttpClientServiceStub()
        {
            _httpClient = new HttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            string content;

            if (requestUri.Contains("cities"))
            {
                content = JsonConvert.SerializeObject(CityOptions.City);
            }
            else
            {
                var getCityDistrictsResponse = new GetCityDistrictsResponse
                {
                    Results = CityDistrictOptions.CityDistricts.Select(x => new CityDistrict
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PolishName = x.PolishName,
                        RowVersion = x.RowVersion,
                        CityId = x.CityId,
                        NameVariants = x.NameVariants
                    }),
                    TotalCount = CityDistrictOptions.CityDistricts.Count
                };
                content = JsonConvert.SerializeObject(getCityDistrictsResponse);
            }

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content)
            };
            return Task.FromResult(httpResponseMessage);
        }
    }
}