using AppX.Client.Domain.Entities.Client;
using AppX.Client.Domain.Entities.UserAccount;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppX.Client.Infrastructure.Persistence
{
    public class ClientsDbContext(DbContextOptions<ClientsDbContext> options) : IdentityDbContext<UserProfile>(options)
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
    }
}
