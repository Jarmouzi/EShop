using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Int64? ParentId { get; set; }
        public int DisplayOrder { get; set; }

        [DisplayName("تایید")]
        public bool Confirmed { get; set; }

        public string? ParentTitle { get; set; }

        public int ParentOrder { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<CategoryViewModel> Categories { get; set; }

    }
}
