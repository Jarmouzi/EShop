using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class ProductVariant_OptionViewModel: BaseViewModel
    { 
		
        [DisplayName("ProductVariantId")]
		public long? ProductVariantId { get; set; }
		
        [DisplayName("Product_OptionId")]
		public long? Product_OptionId { get; set; }

        [ValidateNever]
		public string ProductVariantTitle { get; set; }

        [ValidateNever]
		public string Product_OptionTitle { get; set; }
	}
}