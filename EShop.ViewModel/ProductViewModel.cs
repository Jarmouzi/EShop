using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.ViewModel
{
    public class ProductViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(12)]
        public string Handle { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? Description { get; set; }
		public Int64? CategoryId { get; set; }
		public Int64? BrandId { get; set; }
        public bool? AvailableForSale { get; set; }
        public string? tags { get; set; }
        public bool? Confirmed { get; set; }

        [NotMapped]
        [ValidateNever]
        public string BrandTitle { get; set; }

        [NotMapped]
        [ValidateNever]
        public string CategoryTitle { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<CategoryViewModel> Categories { get; set; }

        [NotMapped]
        [ValidateNever]
        public List<BrandViewModel> Brands { get; set; }
    }
}