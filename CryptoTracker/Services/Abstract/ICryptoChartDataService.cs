using CryptoTracker.Models;

namespace CryptoTracker.Services.Abstract
{
    public interface ICryptoChartDataService
    {
        public Task<CryptoMarketData> GetDataAsync(string coin, string currency, int days);
    }
}
