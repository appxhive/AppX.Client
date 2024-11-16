using AppX.Client.Domain.Constants;
using AppX.Client.Domain.Entities.Client;
using AppX.Client.Domain.Interfaces.AppUsers;
using AppX.Client.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppX.Client.Infrastructure.Repositories.AppUsers
{
    internal class AppUsersRepository(AppUsersDbContext dbContext) : IAppUserRepository
    {
        public async Task<Guid> Create(AppUser entity)
        {
            dbContext.AppUsers.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(AppUser entity)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            var restaurants = await dbContext.AppUsers.ToListAsync();

            return restaurants;
        }

        public async Task<(IEnumerable<AppUser>, int)> GetAllMatchingAsync(
            string? searchPhrase,
            int pageSize,
            int pageNumber,
            string? sortBy,
            SortDirection sortDirection)
        {
            var searchPhraseLower = searchPhrase?.ToLower();

            var baseQuery = dbContext.AppUsers
                .Where(r => searchPhraseLower == null || (r.UserProfile.LastName.ToLower().Contains(searchPhraseLower)
                || r.UserProfile.LastName.ToLower().Contains(searchPhraseLower)));

            var totalCount = await baseQuery.CountAsync();

            if (sortBy != null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<AppUser, object>>>
                {
                    { nameof(AppUser.UserProfile.LastName), r => r.UserProfile.LastName },
                    { nameof(AppUser.ClientProfileId), r => r.ClientProfileId },
                    { nameof(AppUser.UserProfile.Nationality), r => r.UserProfile.Nationality }
                };

                var selectedColumn = columnsSelector[sortBy];

                baseQuery = sortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            //Sample:
            //PageSize = 5, PageNumber = 3: Skip => PageSize * (PageNumber - 1) => 5 * (3-1) => 10

            //1  {...}
            //2  {...}
            //3  {...}
            //4  {...}
            //5  {...}
            //6  {...}
            //7  {...}
            //8  {...}
            //9  {...}
            //10 {...}
            //11 {...}

            return (restaurants, totalCount);
        }

        public async Task<AppUser?> GetByIdAsync(Guid Id)
        {
            var appUser = await dbContext.AppUsers
                .Include(r => r.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return appUser;
        }

        public Task SaveChanges() => dbContext.SaveChangesAsync();
    }
}
