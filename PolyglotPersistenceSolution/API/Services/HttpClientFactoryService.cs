using System.Text.Json;

namespace API.Services
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<T>> ExecuteAsync<T>(string endpoint, int loop = 1)
        {
            var httpClient=_httpClientFactory.CreateClient("GeneratorAPI");
            var result = new List<T>();

            for (int i = 0; i < loop; i++)
            {
                using var response = await httpClient.GetAsync(endpoint,HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream= await response.Content.ReadAsStreamAsync();
                var group = await JsonSerializer.DeserializeAsync<List<T>>(stream,_options);
                result.AddRange(group);
            }

            return result;
        }
    }
}
