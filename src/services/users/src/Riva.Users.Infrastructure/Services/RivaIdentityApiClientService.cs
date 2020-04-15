using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Services;
using Riva.Users.Core.Models;
using Riva.Users.Core.Services;
using Riva.Users.Infrastructure.Models.ApiClientResponses.RivaIdentity;
using Riva.Users.Infrastructure.Models.AppSettings;

namespace Riva.Users.Infrastructure.Services
{
    public class RivaIdentityApiClientService : IRivaIdentityApiClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private const string TokenName = "access_token";
        private const string ApiVersionHeaderName = "api-version";
        private const string ApiVersion = "1.0";

        public RivaIdentityApiClientService(IHttpClientService httpClientService, IHttpContextAccessor httpContextAccessor,
            ILogger logger, IOptions<ApiClientsAppSettings> apiClientsOptions)
        {
            _httpClientService = httpClientService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _httpClientService.BaseAddress = new Uri(apiClientsOptions.Value.RivaIdentityApiUrl);
            _httpClientService.DefaultRequestHeaders.Add(ApiVersionHeaderName, ApiVersion);
        }

        public async Task<IAccount> GetAccountAsync(Guid accountId)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(TokenName);
            _httpClientService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IHttpClientService.AuthScheme, accessToken);

            var response = await _httpClientService.GetAsync($"/api/accounts/{accountId}");
            var responseContentString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<GetAccountResponse>(responseContentString);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var message =
                $"Requesting account data failed. Http status code: {response.StatusCode}. Response content: {responseContentString}.";
            _logger.LogError(ServiceComponentEnumeration.RivaUsers, "Message={message}", message);
            throw new UnprocessableException(message);
        }
    }
}