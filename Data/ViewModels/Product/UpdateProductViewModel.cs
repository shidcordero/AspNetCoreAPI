using System.Collections.Generic;
using Data.ViewModels.Common;

namespace Data.ViewModels.Product
{
    public class UpdateProductViewModel
    {
        public UpdateProductViewModel()
        {
            ValidationResults = new List<ValidationResult>();
        }

        public List<ValidationResult> ValidationResults { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
