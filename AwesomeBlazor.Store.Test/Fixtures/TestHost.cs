using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Diagnostics;

namespace AwesomeBlazor.Store.Test.Fixtures;

internal class TestHost : IAsyncDisposable, IServiceProvider
{
    private XProcess? _azurite;

    private ServiceProvider _serviceProvider;

    public TestHost(Action<IServiceCollection> builder)
    {
        var services = new ServiceCollection();
        builder(services);
        this._serviceProvider = services.BuildServiceProvider();

    }

    public object? GetService(Type serviceType) => this._serviceProvider.GetService(serviceType);

    public async ValueTask StartAzuriteAsync(string? localtion = null)
    {
        this._azurite = XProcess.Start("azurite",
            localtion != null ?
                $"-l \"{localtion}\"" :
                "--inMemoryPersistence");
        await this._azurite.WaitForOutputAsync(output => output.StartsWith("Azurite Table service is successfully listening"), millsecondsTimeout: 5000);
    }

    public async ValueTask DisposeAsync()
    {
        await this._serviceProvider.DisposeAsync();
        this._azurite?.Dispose();
    }
}
