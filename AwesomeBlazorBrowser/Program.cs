using AwesomeBlazor.Store;
using AwesomeBlazorBrowser;
using Azure.Data.Tables;
using SmartComponents.LocalEmbeddings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddTransient(_ => new TableServiceClient(builder.Configuration.GetConnectionString("AwesomeBlazorStore")))
    .AddTransient(_ => new HttpClient())
    .AddSingleton(_ => new LocalEmbedder())
    .AddSingleton<AwesomeBlazorStore>()
    .AddScoped<HelperScriptService>()
    .AddRazorComponents()
    .AddInteractiveServerComponents(); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<AwesomeBlazorBrowser.Components.Index>()
    .AddInteractiveServerRenderMode();

app.Run();
