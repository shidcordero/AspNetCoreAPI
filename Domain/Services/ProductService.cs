using Data.Contracts;
using Data.Utilities;
using System.Threading.Tasks;
using Data.Models.Entities;
using Data.ViewModels.Product;
using Domain.Contracts;

namespace Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Used to Find product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> FindById(int id)
        {
            return await _productRepository.FindById(id);
        }

        /// <summary>
        /// Gets product paginated list
        /// </summary>
        /// <param name="searchViewModel">holds the search parameter</param>
        /// <returns>Paginated List</returns>
        public async Task<Extensions.PaginatedList<Product>> FindProducts(ProductSearchViewModel searchViewModel)
        {
            return await _productRepository.FindProducts(searchViewModel);
        }

        /// <summary>
        /// Used to create Product
        /// </summary>
        /// <param name="product">holds the product data</param>
        public async Task Create(Product product)
        {
            await _productRepository.Create(product);
        }

        /// <summary>
        /// Used to update Product
        /// </summary>
        /// <param name="product">holds the product data</param>
        /// <returns>UpdateProductViewModel</returns>
        public async Task<UpdateProductViewModel> Update(Product product)
        {
            return await _productRepository.Update(product);
        }

        /// <summary>
        /// Used to delete Product
        /// </summary>
        /// <param name="product">holds the product data</param>
        public async Task Delete(Product product)
        {
            await _productRepository.Delete(product);
        }

        /// <summary>
        /// Used to delete Product by id
        /// </summary>
        /// <param name="id">holds the product id</param>
        public async Task DeleteById(int id)
        {
            await _productRepository.DeleteById(id);
        }

        /// <summary>
        /// Checks if product is still exists in the database
        /// </summary>
        /// <param name="name">holds the product name</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsProductExists(string name)
        {
            return await _productRepository.IsProductExists(name);
        }
    }
}