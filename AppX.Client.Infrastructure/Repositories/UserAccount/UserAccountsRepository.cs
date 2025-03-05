using AppX.Client.Domain.Constants;
using AppX.Client.Domain.Entities.UserAccount;
using AppX.Client.Domain.Interfaces.UserAccounts;
using AppX.Client.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppX.Client.Infrastructure.Repositories.UserAccount
{
    internal class UserAccountsRepository(AppxhiveDbContext dbContext) : IUserAccountRepository
    {
        public async Task<Guid> Create(UserProfile entity)
        {
            dbContext.UserProfiles.Add(entity);
            await dbContext.SaveChangesAsync();
            Guid.TryParse(entity.Id, out Guid guid);
            return guid;
        }

        public async Task Delete(UserProfile entity)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            var restaurants = await dbContext.UserProfiles.ToListAsync();

            return restaurants;
        }

        public async Task<(IEnumerable<UserProfile>, int)> GetAllMatchingAsync(
            string? searchPhrase,
            int pageSize,
            int pageNumber,
            string? sortBy,
            SortDirection sortDirection)
        {
            var searchPhraseLower = searchPhrase?.ToLower();

            var baseQuery = dbContext.UserProfiles
                .Where(r => searchPhraseLower == null || (r.LastName.ToLower().Contains(searchPhraseLower)
                || r.LastName.ToLower().Contains(searchPhraseLower)));

            var totalCount = await baseQuery.CountAsync();

            if (sortBy != null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<UserProfile, object>>>
                {
                    { nameof(UserProfile.LastName), r => r.LastName },
                    { nameof(UserProfile.ClientProfileId), r => r.ClientProfileId },
                    { nameof(UserProfile.Nationality), r => r.Nationality }
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

        public async Task<UserProfile?> GetByIdAsync(Guid Id)
        {
            return await dbContext.UserProfiles
                .Include(r => r.ClientProfile)
                .FirstOrDefaultAsync(x => x.Id == $"{Id}");
        }

        public Task SaveChanges() => dbContext.SaveChangesAsync();
    }
}
