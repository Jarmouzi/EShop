using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        //public bool AvailableForSale { get; set; }
        public string? tags { get; set; }
        public bool Confirmed { get; set; }

        [ValidateNever]
        public string BrandTitle { get; set; }

        [ValidateNever]
        public string CategoryTitle { get; set; }

        [NotMapped]
        [ValidateNever]
        public SelectList Categories { get; set; }

        [NotMapped]
        [ValidateNever]
        public SelectList Brands { get; set; }
    }
}