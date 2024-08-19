using ChartJs.Blazor.LineChart;
using CryptoTracker.Models;

namespace CryptoTracker.Utils
{
    public static class ChartDataModifier
    {
        public static void ModifyConfigData(LineConfig config, CryptoMarketData marketData, string selectedCoin, string selectedCurrency)
        {
            while (config.Data.Labels.Count > 0) config.Data.Labels.RemoveAt(0);
            while (config.Data.Datasets.Count > 0) config.Data.Datasets.RemoveAt(0);

            ExtractChartData(marketData, out List<double> chartPrices, out List<string> chartLabels);

            foreach (string label in chartLabels) config.Data.Labels.Add(label);

            config.Data.Datasets.Add(new LineDataset<double>(chartPrices) { Label = $"{selectedCoin} price in {selectedCurrency}".ToUpper() });
        }
        private static void ExtractChartData(CryptoMarketData marketData, out List<double> chartPrices, out List<string> chartLabels)
        {
            List<double> prices = marketData.Prices.Select(p => p[1]).ToList();
            List<string> labels = marketData.Prices.Select(p => DateTimeOffset.FromUnixTimeMilliseconds((long)p[0]).ToLocalTime().ToString("dd.MM.yy")).ToList();

            int pointCount = 20;

            chartPrices = new();
            chartLabels = new();
            int labelCountPerOne = labels.Count() / pointCount;
            int priceCountPerOne = prices.Count() / pointCount;

            for (int i = 0; i < pointCount; i++)
            {
                chartPrices.Add(prices[i * priceCountPerOne]);
                chartLabels.Add(labels[i * labelCountPerOne]);
            }
        }
    }
}
