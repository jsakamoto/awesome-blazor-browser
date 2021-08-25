using AwesomeBlazorBrowser;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
#if DEBUG
using FindRazorSourceFile.WebAssembly;
#endif

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
#if DEBUG
builder.UseFindRazorSourceFile();
#endif
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
await builder.Build().RunAsync();
