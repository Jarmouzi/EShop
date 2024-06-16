using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class Product_OptionViewModel: BaseViewModel
    { 
		
        [DisplayName("ProductId")]
		public long? ProductId { get; set; }
		
        [DisplayName("OptionId")]
		public long? OptionId { get; set; }
		
        [DisplayName("OptionValueId")]
		public long? OptionValueId { get; set; }
		public string ProductTitle { get; set; }
		public string OptionTitle { get; set; }
		public string OptionValueTitle { get; set; }
	}
}