using Data.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Product
{
    public class ProductViewModel
    {

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string ProductDesc { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string ProductImage { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        [SwaggerExclude]
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
    }
}
