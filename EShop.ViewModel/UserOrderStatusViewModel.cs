using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class UserOrderStatusViewModel: BaseViewModel
    { 
		
        [DisplayName("Title")]
		public string? Title { get; set; }
		
        [DisplayName("ClassName")]
		public string? ClassName { get; set; }
	}
}