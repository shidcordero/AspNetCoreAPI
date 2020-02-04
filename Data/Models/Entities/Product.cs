using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Name")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Description")]
        public string ProductDesc { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string ProductImage { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}