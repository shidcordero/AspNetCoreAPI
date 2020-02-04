using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Product;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IProductRepository
    {
        Task<Product> FindById(int id);

        Task<Extensions.PaginatedList<Product>> FindProducts(ProductSearchViewModel searchViewModel);

        Task Create(Product product);

        Task<UpdateProductViewModel> Update(Product product);

        Task Delete(Product product);

        Task DeleteById(int id);

        Task<bool> IsProductExists(string name);
    }
}