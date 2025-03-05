using AppX.Client.API.Extensions;
using AppX.Client.API.Middlewares;
using AppX.Client.Application.Extensions;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Infrastructure.Extensions;
using Serilog;

//#################################################################################

try
{
    Log.Information("AppX Client API startup started...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddInfrastructure(builder.Configuration);

    builder.AddPresentation();

    builder.Services.AddApplication();

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseMiddleware<RequestTimeLoggingMiddleware>();

    app.UseStaticFiles();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints => endpoints.MapControllers());

    app.MapGet("/", () => "AppX Client API");

    //app.MapGroup("api/Identity")
    //.WithTags("Identity")
    //.MapIdentityApi<UserProfile>();

    app.UseEndpoints(endpoints => endpoints.MapControllers());

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "AppX Client API startup failed.");
}
finally
{
    Log.CloseAndFlush();
}

//#################################################################################

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



