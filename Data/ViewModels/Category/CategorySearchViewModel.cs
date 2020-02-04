using Data.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Category
{
    public class CategorySearchViewModel
    {
        [Display(Name = "Find by Category")]
        public string CategoryName { get; set; } = string.Empty;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 15;

        public string SortBy { get; set; } = nameof(CategoryName);

        public string SortOrder { get; set; } = Constants.SortDirection.Ascending;
    }
}