using Data.Models.Entities;
using Data.ViewModels.Common;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICategoryHandler
    {
        Task<ValidationResult> CanAdd(Category category);

        Task<ValidationResult> CanUpdate(int id, Category category);

        Task<ValidationResult> CanDelete(int categoryId);
    }
}