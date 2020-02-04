using Data.Models.Entities;
using Data.Utilities;
using System.Threading.Tasks;
using Data.ViewModels.Product;

namespace Domain.Contracts
{
    public interface IProductService
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