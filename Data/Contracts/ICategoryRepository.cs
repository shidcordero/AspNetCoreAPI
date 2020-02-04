using Data.Models.Entities;
using Data.Utilities;
using System.Threading.Tasks;
using Data.ViewModels.Category;

namespace Data.Contracts
{
    public interface ICategoryRepository
    {
        Task<Category> FindById(int id);

        Task<Extensions.PaginatedList<Category>> FindCategories(CategorySearchViewModel searchViewModel);

        Task Create(Category category);

        Task<UpdateCategoryViewModel> Update(Category category);

        Task Delete(Category category);

        Task DeleteById(int id);

        Task<bool> IsCategoryExists(string name);

        Task<bool> IsCategoryInUsed(int id);
    }
}