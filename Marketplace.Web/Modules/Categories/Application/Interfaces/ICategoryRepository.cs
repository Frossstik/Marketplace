using Marketplace.Web.Modules.Categories.Domain.Entities;

namespace Marketplace.Web.Modules.Categories.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync();
        Task DeleteAsync(Category category);
    }
}
