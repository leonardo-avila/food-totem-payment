using Newtonsoft.Json;
using System.Text;

namespace FoodTotem.Payment.Gateways.Http
{
    [ExcludeFromCodeCoverage]
	public class HttpHandler : IHttpHandler
	{
        private readonly HttpClient _httpClient;

		public HttpHandler()
		{
            _httpClient = new HttpClient();
		}

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers)
        {
            HttpRequestMessage request = new(HttpMethod.Get, url);
            AddHeadersToRequest(request, headers);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            T responseObject = JsonConvert.DeserializeObject<T>(responseContent);

            return responseObject;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data, Dictionary<string, string> headers)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpRequestMessage request = new(HttpMethod.Post, url)
            {
                Content = content
            };
            AddHeadersToRequest(request, headers);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseContent);

            return responseObject;
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest data, Dictionary<string, string> headers)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpRequestMessage request = new(HttpMethod.Put, url)
            {
                Content = content
            };
            AddHeadersToRequest(request, headers);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseContent);

            return responseObject;
        }

        public async Task<string> DeleteAsync(string url, Dictionary<string, string> headers)
        {
            HttpRequestMessage request = new(HttpMethod.Delete, url);
            AddHeadersToRequest(request, headers);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static void AddHeadersToRequest(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}