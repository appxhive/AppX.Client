using AppX.Client.API.Extensions;
using AppX.Client.API.Middlewares;
using AppX.Client.Application.Extensions;
using AppX.Client.Domain.Entities.AppUser;
using AppX.Client.Infrastructure.Extensions;
using Serilog;

try
{
    Log.Information("AppX Clients API startup started...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddInfrastructure(builder.Configuration);

    builder.AddPresentation();

    builder.Services.AddApplication();

    var app = builder.Build();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapGet("/", () => "AppX Clients API");

    app.MapGroup("api/IdentityUser")
    .WithTags("AppX-IdentityUser Endpoints")
    .MapIdentityApi<UserProfile>();

    //app.MapGroup("api/Client")
    //.WithTags("AppX-Client Endpoints")
    //.MapIdentityApi<ClientProfile>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "AppX Clients API startup failed.");
}
finally
{
    Log.CloseAndFlush();
}

/*** Use this when creating Database thru EF core CLI or dotnet EF Core CLI****
 * 
 * Drop existing DB and delete migration files.
 
try
{
    Log.Information("AppX Clients API startup started...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "AppX Clients API startup failed.");
}
finally
{
    Log.CloseAndFlush();
}
 
 */

