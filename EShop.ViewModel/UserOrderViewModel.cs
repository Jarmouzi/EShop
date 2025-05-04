using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class UserOrderViewModel: BaseViewModel
    { 
		
        [DisplayName("Handle")]
		public string? Handle { get; set; }
		
        [DisplayName("UserId")]
		public Guid? UserId { get; set; }
		
        [DisplayName("CartId")]
		public long? CartId { get; set; }
		
        [DisplayName("UserAddressId")]
		public long? UserAddressId { get; set; }
		
        [DisplayName("UserOrderStatusId")]
		public long? UserOrderStatusId { get; set; }
		
        [DisplayName("PaymentGatewayId")]
		public long? PaymentGatewayId { get; set; }
		
        [DisplayName("PaymentConfirmationCode")]
		public string? PaymentConfirmationCode { get; set; }

        [ValidateNever]
		public string UserOrderStatusTitle { get; set; }
	}
}