using Undersoft.GCC.Service.API.Extensions;
using Undersoft.GCC.Service.Extensions;
using Undersoft.SDK.Service.Data.Store;
using Undersoft.SDK.Service.Server;
using Undersoft.SDK.Service.Server.Hosting;

namespace Undersoft.GCC.Service.API;

public class Setup
{
    public void ConfigureServices(IServiceCollection srvc)
    {
        srvc.AddServerSetup()
            .ConfigureServer(true)
            .AddDataServer<IEntityStore>()
            .AddDataServer<IEventStore>()
            .AddCurrencyContexts()
            .AddCurrencyWorkflows();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseServerSetup(env)
            .UseServiceServer(false, ["v1"])
            .UseInternalProvider()
            .UseDataMigrations()
            .UseCurrenciesFeed()
            .UseServiceClients();
    }
}
