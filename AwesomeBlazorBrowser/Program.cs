using AwesomeBlazorBrowser;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
ConfigureServices(builder.Services, builder.HostEnvironment);
await builder.Build().RunAsync();

void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
{
    services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) });
    services.AddScoped<HelperScriptService>();
}