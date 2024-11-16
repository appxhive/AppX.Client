using AppX.Client.Domain.Constants;
using AppX.Client.Domain.Entities.Client;

namespace AppX.Client.Domain.Interfaces.AppUsers
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<(IEnumerable<AppUser>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
        Task<AppUser?> GetByIdAsync(Guid Id);
        Task<Guid> Create(AppUser entity);
        Task Delete(AppUser entity);
        //Task<Restaurant?> UpdateByIdAsync(Restaurant entity);
        Task SaveChanges();
    }
}
