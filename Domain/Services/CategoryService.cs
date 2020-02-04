using Data.Contracts;
using Data.Utilities;
using Domain.Contracts;
using System.Threading.Tasks;
using Data.Models.Entities;
using Data.ViewModels.Category;

namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Used to Find category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> FindById(int id)
        {
            return await _categoryRepository.FindById(id);
        }

        /// <summary>
        /// Gets category paginated list
        /// </summary>
        /// <param name="searchViewModel">holds the search parameter</param>
        /// <returns>Paginated List</returns>
        public async Task<Extensions.PaginatedList<Category>> FindCategories(CategorySearchViewModel searchViewModel)
        {
            return await _categoryRepository.FindCategories(searchViewModel);
        }

        /// <summary>
        /// Used to create Category
        /// </summary>
        /// <param name="category">holds the category data</param>
        public async Task Create(Category category)
        {
            await _categoryRepository.Create(category);
        }

        /// <summary>
        /// Used to update Category
        /// </summary>
        /// <param name="category">holds the category data</param>
        /// <returns>UpdateCategoryViewModel</returns>
        public async Task<UpdateCategoryViewModel> Update(Category category)
        {
            return await _categoryRepository.Update(category);
        }

        /// <summary>
        /// Used to delete Category
        /// </summary>
        /// <param name="category">holds the category data</param>
        public async Task Delete(Category category)
        {
            await _categoryRepository.Delete(category);
        }

        /// <summary>
        /// Used to delete Category by id
        /// </summary>
        /// <param name="id">holds the category id</param>
        public async Task DeleteById(int id)
        {
            await _categoryRepository.DeleteById(id);
        }

        /// <summary>
        /// Checks if category is still exists in the database
        /// </summary>
        /// <param name="name">holds the category name</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsCategoryExists(string name)
        {
            return await _categoryRepository.IsCategoryExists(name);
        }

        /// <summary>
        /// Checks if category is still in use
        /// </summary>
        /// <param name="id">holds the category id</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsCategoryInUsed(int id)
        {
            return await _categoryRepository.IsCategoryInUsed(id);
        }
    }
}