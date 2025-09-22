using clipVault.Models.Categories;
using clipVault.Models.Categories.GetVideoCategories;

namespace clipVault.Repositories.Categories
{
    public interface IVideoCategoryService
    {
        Task<List<VideoCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    }
}
