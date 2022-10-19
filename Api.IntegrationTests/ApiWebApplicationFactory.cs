using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Api.IntegrationTests;

public class ApiWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly string _environment;

    public ApiWebApplicationFactory(string environment = "IntegrationTests")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IExternalService));
            services.TryAddTransient<IExternalService, MockExternalService>();
        });

        return base.CreateHost(builder);
    }
}