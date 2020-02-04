using System.Collections.Generic;
using Data.ViewModels.Common;

namespace Data.ViewModels.Category
{
    public class UpdateCategoryViewModel
    {
        public UpdateCategoryViewModel()
        {
            ValidationResults = new List<ValidationResult>();
        }

        public List<ValidationResult> ValidationResults { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
