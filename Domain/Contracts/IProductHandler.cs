using Data.Models.Entities;
using Data.ViewModels.Common;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IProductHandler
    {
        Task<ValidationResult> CanAdd(Product product);

        Task<ValidationResult> CanUpdate(int id, Product product);

        Task<ValidationResult> CanDelete(int productId);
    }
}