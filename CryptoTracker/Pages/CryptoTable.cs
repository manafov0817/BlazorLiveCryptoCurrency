using CryptoTracker.Models;
using Microsoft.AspNetCore.Components;

namespace CryptoTracker.Pages
{
    public partial class CryptoTable : IDisposable
    {
        [Inject]
        private CryptoWebSocketService WebSocketService { get; set; }
        private int[] pageSizeOptions = { 5, 10, 15, 20 };
        private List<CryptoTableRow> cryptoCurrencies = new();
        private string SelectedCurrency = "USD";
        string[] Currencies = { "USD", "EUR", "GBP" };
        protected override async void OnInitialized()
        {
            LoadDataAsync();
        }
        private void SetCurrency(string currency)
        {
            SelectedCurrency = currency;
            WebSocketService.SetOptionsAsync(SelectedCurrency);
        }
        private async void LoadDataAsync()
        {
            WebSocketService.OnDataReceived += UpdateCryptoCurrencies;
            await WebSocketService.ConnectAsync();
        }
        public void Dispose()
        {
            WebSocketService.OnDataReceived -= UpdateCryptoCurrencies;
            WebSocketService.Dispose();
        }
        private void UpdateCryptoCurrencies(List<CryptoTableRow> data)
        {
            cryptoCurrencies = data;
            InvokeAsync(StateHasChanged);
        }
    }

    public class CryptoPrice
    {
        public string ProductId { get; set; }
        public int Price { get; set; }
    }
}
