using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using CryptoTracker.Models;
using CryptoTracker.Services.Abstract;
using CryptoTracker.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Net.Http.Json;
using static MudBlazor.CategoryTypes;

namespace CryptoTracker.Pages
{
    public partial class CryptoChart
    {
        [Inject]
        private ICryptoChartDataService _cryptoChartDataService { get; set; }

        private LineConfig _config = new LineConfig
        {
            Options = new LineOptions
            {
                AspectRatio = 2.2,
                MaintainAspectRatio = true,
                Responsive = true,
            }
        };

        private readonly string[] Coins = { "bitcoin", "ethereum", "cardano", "avalanche-2", "dogecoin", "binancecoin", "solana", "polkadot", "ripple", "litecoin", "chainlink", "stellar", "matic-network", "vechain", "tron", "tezos", "cosmos", "monero", "eos", "iota", "neo", "dash", "zcash" };
        private readonly string[] Currencies = { "usd", "eur", "gbp", "jpy", "aud", "cad", "chf", "cny", "hkd", "nzd", "sgd", "sek", "nok", "mxn", "inr", "rub", "zar", "try", "brl", "twd" };

        private readonly FormDataHolder _formDataHolder = new();

        private bool timeLimitExceeded = false;
        MudButton _dayButton, _weekButton, _monthButton, _yearButton;
        List<MudButton> _buttons;
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _weekButton.Color = Color.Primary;
                _buttons = new() { _dayButton, _weekButton, _monthButton, _yearButton };
            }
        }
        async void HandleClick(MouseEventArgs e, MudButton clickedButton, int dayCount)
        {

            foreach (var button in _buttons)
                if (button.Equals(clickedButton)) button.Color = Color.Primary;
                else button.Color = Color.Default;
            _formDataHolder.SelectedDays = dayCount;
            await LoadDataAsync();
        }

        private void CloseMe(bool value)
        {
            if (value) timeLimitExceeded = false;
        }
        protected override async void OnInitialized()
        {
            LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            CryptoMarketData chartData = new();

            try
            {
                chartData = await _cryptoChartDataService.GetDataAsync(_formDataHolder.SelectedCoin, _formDataHolder.SelectedCurrency, _formDataHolder.SelectedDays);
                ChartDataModifier.ModifyConfigData(_config, chartData, _formDataHolder.SelectedCoin, _formDataHolder.SelectedCurrency);
                timeLimitExceeded = false;
            }
            catch
            {
                timeLimitExceeded = true;
            }
            StateHasChanged();
        }
    }
}
