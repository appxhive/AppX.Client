using AppX.Client.Domain.Entities.Client;
using AppX.Client.Domain.Entities.AppUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppX.Client.Infrastructure.Persistence
{
    public class AppUsersDbContext(DbContextOptions<AppUsersDbContext> options) : IdentityDbContext<UserProfile>(options)
    {
        internal DbSet<AppUser> AppUsers { get; set; }
        internal DbSet<ClientProfile> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasOne(x => x.UserProfile)
                .WithOne()
                .HasForeignKey<AppUser>(x => x.UserProfileId);

            builder.Entity<AppUser>()
                .HasOne(x => x.ClientProfile)
                .WithOne()
                .HasForeignKey<AppUser>(x => x.ClientProfileId);
        }
    }
}
