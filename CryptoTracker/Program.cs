using CryptoTracker;
using CryptoTracker.Services.Abstract;
using CryptoTracker.Services.Concrete;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient<ICryptoChartDataService, CryptoChartDataService>(client => client.BaseAddress = new Uri("https://api.coingecko.com/"));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<CryptoWebSocketService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
