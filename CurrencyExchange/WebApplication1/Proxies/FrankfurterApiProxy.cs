using CurrencyExchangeApi.Dtos.Response;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchangeApi.Proxies
{
    public interface IFrankfurterApiProxy
    {
        Task<ExchangeRateResponse?> GetExchangeRateAsync(string from, string to);
    }

    public class FrankfurterApiProxy : IFrankfurterApiProxy
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public FrankfurterApiProxy(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<ExchangeRateResponse?> GetExchangeRateAsync(string sellCurrency, string buyCurrency)
        {
            var cacheKey = $"Frankfurter_{sellCurrency}_{buyCurrency}";
            if (_cache.TryGetValue(cacheKey, out ExchangeRateResponse? cachedResult))
            {
                return cachedResult;
            }

            var url = $"https://api.frankfurter.app/latest?amount=1&from={sellCurrency}&to={buyCurrency}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result != null)
            {
                // Cache for 10 minutes
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(20));
            }

            return result;
        }
    }
}
