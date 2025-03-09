using AppX.Client.API.Middlewares;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

namespace AppX.Client.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        //Separating services used by presentation modules
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //for empty queryString => reset password

            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            builder.Services.AddAuthentication();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(config =>
            {
                //Add Authorize - passing token
                config.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    //Use Http - token
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                //Add token to every request: in the header request - Authorization: Bearer "token"
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth" }
                        },
                        []
                    }
                });
            });

            builder.Services.AddScoped<CustomHttpContextMiddleware>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
                //config
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                //.WriteTo.File("Logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                //.WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine}{Message:lj}{NewLine}{Exception}");
            });

        }
    }
}
