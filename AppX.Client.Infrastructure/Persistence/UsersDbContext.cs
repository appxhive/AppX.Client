using AppX.Client.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Restaurants.Infrastructure.Persistence
{
    internal class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext<User>(options)
    {
        internal DbSet<User> Users { get; set; }
    }
}
