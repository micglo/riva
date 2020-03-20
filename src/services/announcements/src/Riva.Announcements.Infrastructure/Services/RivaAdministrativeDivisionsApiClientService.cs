using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Riva.Announcements.Core.Models;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions;
using Riva.Announcements.Infrastructure.Models.AppSettings;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.Announcements.Infrastructure.Services
{
    public class RivaAdministrativeDivisionsApiClientService : IRivaAdministrativeDivisionsApiClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private const string TokenName = "access_token";
        private const string ApiVersionHeaderName = "api-version";
        private const string ApiVersion = "1.0";

        public RivaAdministrativeDivisionsApiClientService(IHttpClientService httpClientService, IHttpContextAccessor httpContextAccessor,
            ILogger logger, IOptions<ApiClientsAppSettings> apiClientsOptions)
        {
            _httpClientService = httpClientService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _httpClientService.BaseAddress = new Uri(apiClientsOptions.Value.RivaAdministrativeDivisionsApiUrl);
            _httpClientService.DefaultRequestHeaders.Add(ApiVersionHeaderName, ApiVersion);
        }

        public async Task<ICity> GetCityAsync(Guid cityId)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(TokenName);
            _httpClientService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IHttpClientService.AuthScheme, accessToken);

            var response = await _httpClientService.GetAsync($"/api/cities/{cityId}");
            var responseContentString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<GetCityResponse>(responseContentString);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var message =
                $"Requesting city data failed. Http status code: {response.StatusCode}. Response content: {responseContentString}.";
            _logger.LogError(ServiceComponentEnumeration.RivaAnnouncements, "Message={message}", message);
            throw new UnprocessableException(message);
        }

        public async Task<IEnumerable<ICityDistrict>> GetCityDistrictsAsync(Guid cityId)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(TokenName);
            _httpClientService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IHttpClientService.AuthScheme, accessToken);

            var response = await _httpClientService.GetAsync($"/api/cityDistricts?cityId={cityId}");
            var responseContentString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var getCityDistrictsResponse = JsonConvert.DeserializeObject<GetCityDistrictsResponse>(responseContentString);
                return getCityDistrictsResponse.Results;
            }

            var message = $"Requesting cityDistricts data failed. Http status code: {response.StatusCode}. Response content: {responseContentString}.";
            _logger.LogError(ServiceComponentEnumeration.RivaAnnouncements, "Message={message}", message);
            throw new UnprocessableException(message);
        }
    }
}