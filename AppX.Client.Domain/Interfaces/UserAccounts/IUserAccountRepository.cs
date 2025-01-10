using AppX.Client.Domain.Constants;
using AppX.Client.Domain.Entities.UserAccount;

namespace AppX.Client.Domain.Interfaces.UserAccounts
{
    public interface IUserAccountRepository
    {
        Task<IEnumerable<UserProfile>> GetAllAsync();
        Task<(IEnumerable<UserProfile>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
        Task<UserProfile?> GetByIdAsync(Guid Id);
        Task<Guid> Create(UserProfile entity);
        Task Delete(UserProfile entity);
        //Task<Restaurant?> UpdateByIdAsync(Restaurant entity);
        Task SaveChanges();
    }
}
