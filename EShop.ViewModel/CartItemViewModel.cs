using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class CartItemViewModel: BaseViewModel
    { 
		
        [DisplayName("CartId")]
		public long? CartId { get; set; }
		
        [DisplayName("Amount")]
		public long? Amount { get; set; }
		
        [DisplayName("DiscountAmount")]
		public long? DiscountAmount { get; set; }
		
        [DisplayName("Quantity")]
		public int? Quantity { get; set; }
		
        [DisplayName("ProductVarientId")]
		public long? ProductVarientId { get; set; }
		
        [DisplayName("ProductHandle")]
		public string? ProductHandle { get; set; }
		
        [DisplayName("ProductUrl")]
		public string? ProductUrl { get; set; }

        [DisplayName("ProductUrl")]
        public bool? SupplierConfirmed { get; set; }

        [ValidateNever]
		public string CartTitle { get; set; }
	}
}