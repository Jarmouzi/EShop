using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public bool Confirmed { get; set; }
		public string ProductTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string OptionTitle { get; set; }
        public string ValueTitle { get; set; }


        [NotMapped]
        [BindProperty]
        [DisplayName("تصویر")]
        public IFormFile UploadedFile { get; set; }
    }
}