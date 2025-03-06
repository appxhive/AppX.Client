using AppX.Client.Domain.Entities.Client;
using AppX.Client.Domain.Entities.UserAccount;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AppX.Client.Infrastructure.Persistence
{
    public class AppxhiveDbContextFactory : IDesignTimeDbContextFactory<AppxhiveDbContext>
    {
        public AppxhiveDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppxhiveDbContext>();
            var connectionString = configuration.GetConnectionString("AppxhivePostgreSqlDb");

            builder.UseNpgsql(connectionString);

            return new AppxhiveDbContext(builder.Options);
        }
    }

    public class AppxhiveDbContext(DbContextOptions<AppxhiveDbContext> options) : IdentityDbContext<UserProfile>(options)
    {
        internal DbSet<UserProfile> UserProfiles { get; set; }
        internal DbSet<ClientProfile> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>()
                .HasOne(x => x.ClientProfile)
                .WithOne()
                .HasForeignKey<UserProfile>(x => x.ClientProfileId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=AppxhiveDb;Username=postgres;Password=112233Abc");
            }
        }
    }


}
