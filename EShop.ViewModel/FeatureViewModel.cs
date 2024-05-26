using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.ViewModel
{
    public class FeatureViewModel: BaseViewModel
    {
        [DisplayName("گروه محصول")]
        public Int64? CategoryId { get; set; }

        [DisplayName("گروه مشخصات")]
        public Int64? ParentId { get; set; }

        [Required(ErrorMessage = "لطفا نام مشخصه را وارد نمایید", AllowEmptyStrings = false)]
        [DisplayName("نام مشخصه")]
        public string? Title { get; set; }

        [DisplayName("گروه مشخصات")]
        public string? ParentTitle { get; set; }

        [DisplayName("گروه محصول")]
        public string? CategoryTitle { get; set; }

        [DisplayName("آیکون")]
        public string? Icon { get; set; }

        [DisplayName("تایید")]
        public bool Confirmed { get; set; }

        [NotMapped]
        public SelectList Categories { get; set; }

        [NotMapped]
        public SelectList Parents { get; set; }
    }
}