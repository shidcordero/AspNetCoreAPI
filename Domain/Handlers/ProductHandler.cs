using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Common;
using Domain.Contracts;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class ProductHandler : IProductHandler
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Instantiate ProductService
        /// </summary>
        /// <param name="productService">service instance</param>
        public ProductHandler(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Used to validate if product data can be added in the database
        /// </summary>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanAdd(Product product)
        {
            ValidationResult validationResult = null;

            if (product != null)
            {
                if (await _productService.IsProductExists(product.ProductName))
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordExists);
                }
                var category = await _categoryService.FindById(product.CategoryId);
                if (category == null)
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorCategoryRecordExists);
                }
            }
            else
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordInvalid);
            }
            return validationResult;
        }

        /// <summary>
        /// Used to validate if product data can be updated in the database
        /// </summary>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanUpdate(int id, Product product)
        {
            ValidationResult validationResult = null;

            if (product != null)
            {
                var dbProduct = await _productService.FindById(id);
                if (dbProduct != null)
                {
                    if (!product.ProductName.Equals(dbProduct.ProductName) && await _productService.IsProductExists(product.ProductName))
                    {
                        validationResult = new ValidationResult(Constants.Message.ErrorRecordExists);
                    }

                    var category = await _categoryService.FindById(product.CategoryId);
                    if (category == null)
                    {
                        validationResult = new ValidationResult(Constants.Message.ErrorCategoryRecordExists);
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
        /// Used to validate if product data can be deleted in the database
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanDelete(int productId)
        {
            ValidationResult validationResult = null;
            var product = await _productService.FindById(productId);

            if (product == null)
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordNotExists);
            }
            return validationResult;
        }
    }
}