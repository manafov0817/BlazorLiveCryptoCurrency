using CryptoTracker.Services.Abstract;
using CryptoTracker.Utils;
using System.Net.Http.Json;

namespace CryptoTracker.Services.Concrete
{
    public class CryptoChartDataService : ICryptoChartDataService
    {
        private readonly HttpClient _httpClient;
        public CryptoChartDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CryptoMarketData> GetDataAsync(string coin, string currency, int days)
        {
            string url = $"api/v3/coins/{coin}/market_chart?vs_currency={currency}&days={days}";
            return await _httpClient.GetFromJsonAsync<CryptoMarketData>(url);
        }
    }
}
