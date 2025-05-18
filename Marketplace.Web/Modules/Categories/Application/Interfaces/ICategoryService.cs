using Marketplace.Web.Modules.Categories.Domain.Entities;

namespace Marketplace.Web.Modules.Categories.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetOrCreateAsync(string categoryName, Guid? creatorSellerId);
    }
}
