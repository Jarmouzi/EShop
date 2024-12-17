using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class PaymentGatewayViewModel: BaseViewModel
    { 
		
        [DisplayName("Title")]
		public string? Title { get; set; }
		
        [DisplayName("Description")]
		public string? Description { get; set; }
		
        [DisplayName("Url")]
		public string? Url { get; set; }
		
        [DisplayName("IsPublic")]
		public bool? IsPublic { get; set; }
	}
}