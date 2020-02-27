using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Core.Services
{
    public interface IHttpClientService
    {
        public const string AuthScheme = "Bearer";
        public Uri BaseAddress { get; set; }
        public HttpRequestHeaders DefaultRequestHeaders { get; }
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}