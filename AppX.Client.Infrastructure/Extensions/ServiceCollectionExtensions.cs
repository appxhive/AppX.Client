using AppX.Client.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //var connectionStringMsSql = configuration.GetConnectionString("ClientsMsSqlDb");
        //services.AddDbContext<UsersDbContext>(
        //    options => 
        //    options.UseSqlServer(connectionStringMsSql)
        //    );//.EnableSensitiveDataLogging()

        var connectionStringMySql = configuration.GetConnectionString("ClientsMySqlDb");
        services.AddDbContext<UsersDbContext>(
            options =>
            options.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql),
            options => options.EnableRetryOnFailure())); //new MySqlServerVersion(new Version(8, 0, 23))

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()//support role based claim authentication
            //.AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<UsersDbContext>();
    }
}
