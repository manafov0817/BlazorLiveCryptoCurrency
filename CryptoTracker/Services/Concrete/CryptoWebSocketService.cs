using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using CryptoTracker.Models; 
public class CryptoWebSocketService : IDisposable
{
    private ClientWebSocket _webSocket;
    public event Action<List<CryptoTableRow>> OnDataReceived;
    public string CurrentCurrency = "USD";
    private List<CryptoTableRow> _realtimeData = new();   
    public async Task ConnectAsync()
    {
        var uri = new Uri("wss://ws-feed.pro.coinbase.com");

        _webSocket = new ClientWebSocket();

        await _webSocket.ConnectAsync(uri, CancellationToken.None);

        SetOptionsAsync("USD");

        await ReceiveMessagesAsync();
    }
    public async void SetOptionsAsync(string currency)
    {
        Unsubscribe();
        Subscribe(currency);
    }
    public async void Subscribe(string currency)
    {
        string[] strArr = GetProductIds(1, 20, currency);
        var subscribeMessage = $@"{{
            ""type"": ""subscribe"",
            ""channels"": [{{
                ""name"": ""ticker"", 
                ""product_ids"": [{string.Join(", ", strArr.Select(s => $"\"{s}\""))}]
                }}]
            }}";

        await _webSocket.SendAsync(Encoding.UTF8.GetBytes(subscribeMessage), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    public async void Unsubscribe()
    {
        var unsubscribeMessage = $@"{{
            ""type"": ""unsubscribe"",
            ""channels"": [{{
                ""name"": ""ticker"", 
                ""product_ids"": []
                }}]
            }}";

        await _webSocket.SendAsync(Encoding.UTF8.GetBytes(unsubscribeMessage), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024 * 4];

        while (_webSocket != null && _webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);

                ProcessMessage(json);
                OnDataReceived?.Invoke(_realtimeData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    private void ProcessMessage(string json)
    {
        var data = JsonSerializer.Deserialize<CryptoTableRow>(json);

        if (data == null || data.product_id == null) return;

        if (!data.product_id.ToLower().Contains(CurrentCurrency.ToLower()))
        {
            CurrentCurrency = data.product_id.Split('-')[1];
            _realtimeData = new();
        }

        int index = _realtimeData.FindIndex(c => c.product_id == data.product_id);

        if (index != -1)
        {
            _realtimeData[index] = data;
        }
        else
        {
            _realtimeData.Add(data);
        }
    }
    public async void Dispose()
    {
        if (_webSocket != null)
        {
            Unsubscribe();
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", CancellationToken.None);
            _webSocket.Dispose();
            _webSocket = null;
        }
    }
    private string[] GetProductIds(int page, int take, string currency)
    {
        string[] allProductIds = {
                $"BTC-{currency}",
                $"XLM-{currency}",
                $"APT-{currency}",
                $"FET-{currency}",
                $"ETH-{currency}",
                $"USDT-{currency}",
                $"XRP-{currency}",
                $"ADA-{currency}",
                $"DOGE-{currency}",
                $"SOL-{currency}",
                $"DOT-{currency}",
                $"SHIB-{currency}",
                $"LTC-{currency}",
                $"AVAX-{currency}",
                $"ATOM-{currency}",
                $"LINK-{currency}",
                $"ETC-{currency}",
                $"BCH-{currency}",
                $"MATIC-{currency}"
              };
        string[] result = new string[take];

        return allProductIds.Skip((page - 1) * take).Take(take).ToArray();
    }
}