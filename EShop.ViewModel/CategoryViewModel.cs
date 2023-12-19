using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string? Path { get; set; }
        public Byte Level { get; set; }

        [NotMapped]
        public Guid? GrandParentId { get; set; }
        public Guid? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool Confirmed { get; set; }

        [NotMapped]
        public string? ParentTitle { get; set; }

        [NotMapped]
        [ValidateNever]
        public SelectList PrimaryCategories { get; set; }
        [NotMapped]
        [ValidateNever]
        public SelectList SecondaryCategories { get; set; }
    }
}
