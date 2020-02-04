using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Common;
using Domain.Contracts;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class CategoryHandler: ICategoryHandler
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Instantiate CategoryService
        /// </summary>
        /// <param name="categoryService">service instance</param>
        public CategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Used to validate if category data can be added in the database
        /// </summary>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanAdd(Category category)
        {
            ValidationResult validationResult = null;

            if (category != null)
            {
                if (await _categoryService.IsCategoryExists(category.CategoryName))
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordExists);
                }
            }
            else
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordInvalid);
            }
            return validationResult;
        }

        /// <summary>
        /// Used to validate if category data can be updated in the database
        /// </summary>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanUpdate(int id, Category category)
        {
            ValidationResult validationResult = null;

            if (category != null)
            {
                var dbCategory = await _categoryService.FindById(id);
                if (dbCategory != null)
                {
                    if (!category.CategoryName.Equals(dbCategory.CategoryName) && await _categoryService.IsCategoryExists(category.CategoryName))
                    {
                        validationResult = new ValidationResult(Constants.Message.ErrorRecordExists);
                    }
                }
                else
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordNotExists);
                }
            }
            else
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordInvalid);
            }
            return validationResult;
        }

        /// <summary>
        /// Used to validate if category data can be deleted in the database
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanDelete(int categoryId)
        {
            ValidationResult validationResult = null;
            var category = await _categoryService.FindById(categoryId);

            if (category == null)
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordNotExists);
            }
            else
            {
                if (await _categoryService.IsCategoryInUsed(categoryId))
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordInUse);
                }
            }
            return validationResult;
        }
    }
}