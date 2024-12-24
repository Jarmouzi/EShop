using EShop.Utilities;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModel
{
    public class BrandViewModel: BaseViewModel
    {
        [Required]
        [MaxLength(12)]
        public string Handle { get; set; } = string.Empty;
        public string? Title { get; set; }

        public string? Title_En { get; set; }
        public string? Logo { get; set; }
        public string? Banner { get; set; }
        public bool? Confirmed { get; set; }
	}
}