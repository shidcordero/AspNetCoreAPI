using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}