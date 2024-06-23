using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.ViewModel
{
    public class Product_ImageViewModel: BaseViewModel
    { 
		public long ProductId { get; set; }
		public long? Product_OptionId { get; set; }
		public long? ImageId { get; set; }

        [DefaultValue(false)]
        public bool Featured { get; set; }
        [DefaultValue(false)]
        public bool Confirmed { get; set; }

        [ValidateNever]
        public string ProductTitle { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public string ImageAlt { get; set; }

        [ValidateNever]
        public string OptionTitle { get; set; }

        [ValidateNever]
        public string ValueTitle { get; set; }


        [NotMapped]
        [BindProperty]
        [DisplayName("تصویر")]
        public IFormFile UploadedFile { get; set; }
    }
}