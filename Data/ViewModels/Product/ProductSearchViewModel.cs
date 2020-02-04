using Data.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Product
{
    public class ProductSearchViewModel
    {
        [Display(Name = "Find by Product")]
        public string ProductName { get; set; } = string.Empty;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 15;

        public string SortBy { get; set; } = nameof(ProductName);

        public string SortOrder { get; set; } = Constants.SortDirection.Ascending;
    }
}