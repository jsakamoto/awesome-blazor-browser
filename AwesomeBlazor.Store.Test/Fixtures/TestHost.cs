using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Diagnostics;

namespace AwesomeBlazor.Store.Test.Fixtures;

internal class TestHost : IDisposable, IServiceProvider
{
    private XProcess? _azurite;

    private IServiceProvider _serviceProvider;

    public TestHost(Action<IServiceCollection> builder)
    {
        var services = new ServiceCollection();
        builder(services);
        this._serviceProvider = services.BuildServiceProvider();
    }

    public object? GetService(Type serviceType) => this._serviceProvider.GetService(serviceType);

    public async ValueTask StartAzuriteAsync()
    {
        this._azurite = XProcess.Start("azurite", "--inMemoryPersistence");
        await this._azurite.WaitForOutputAsync(output => output.StartsWith("Azurite Table service is successfully listening"), millsecondsTimeout: 5000);
    }

    public void Dispose()
    {
        this._azurite?.Dispose();
    }

}
