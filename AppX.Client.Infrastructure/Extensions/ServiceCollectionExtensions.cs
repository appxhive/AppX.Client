using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Entities.Client;
using AppX.Client.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppX.Client.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //MSSQL :
        //var connectionStringMsSql = configuration.GetConnectionString("ClientsMsSqlDb");
        //services.AddDbContext<UsersDbContext>(
        //    options =>
        //    options.UseSqlServer(connectionStringMsSql)
        //    );//.EnableSensitiveDataLogging()

        //MYSQL :
        var connectionStringMySql = configuration.GetConnectionString("ClientsMySqlDb");
        services.AddDbContext<ClientsDbContext>(
            options =>
            options.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql),
            options => options.EnableRetryOnFailure())); //new MySqlServerVersion(new Version(8, 0, 23))

        //Identity : 
        services.AddIdentityApiEndpoints<UserProfile>()
            .AddRoles<IdentityRole>()//support role based claim authentication
                                     //.AddClaimsPrincipalFactory<ClientsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<ClientsDbContext>()
            .AddDefaultTokenProviders();

        //services.AddIdentityCore<UserProfile>(opt =>
        //{
        //    opt.Password.RequireNonAlphanumeric = false;
        //})
        //.AddRoles<IdentityRole>()
        //.AddRoleManager<RoleManager<IdentityRole>>()
        //.AddEntityFrameworkStores<AppUsersDbContext>();

        //services.AddIdentity<UserProfile, IdentityRole>()
        //    .AddEntityFrameworkStores<AppUsersDbContext>()
        //    .AddDefaultTokenProviders();
    }
}
