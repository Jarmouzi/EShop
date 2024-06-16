using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class Product_Variant_OptionViewModel: BaseViewModel
    { 
		
        [DisplayName("ProductVariantId")]
		public long? ProductVariantId { get; set; }
		
        [DisplayName("Product_OptionId")]
		public long? Product_OptionId { get; set; }
		public string ProductVariantTitle { get; set; }
		public string Product_OptionTitle { get; set; }
	}
}