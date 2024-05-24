using EShop.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.ViewModel
{
    public class BannerViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? Path { get; set; }
		public bool Confirmed { get; set; }

        [NotMapped]
        [BindProperty]
        [DisplayName("تصویر بنر")]
        public IFormFile UploadedFile { get; set; }
    }
}