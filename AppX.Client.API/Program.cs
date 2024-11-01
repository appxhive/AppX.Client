using Restaurants.Infrastructure.Extensions;
using Serilog;

try
{
    Log.Information("AppX Client API startup started...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.MapGet("/", () => "AppX Client API");

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "AppX Client API startup failed.");
}
finally
{
    Log.CloseAndFlush();
}