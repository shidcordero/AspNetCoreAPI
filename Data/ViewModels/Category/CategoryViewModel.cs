using Data.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string CategoryName { get; set; }

        [SwaggerExclude]
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
    }
}
