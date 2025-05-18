using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Categories.Domain.Enums;

namespace Marketplace.Web.Modules.Categories.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync(CategoryStatusEnum? status);
        Task DeleteAsync(Category category);
    }
}
