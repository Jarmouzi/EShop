using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class UserProfileViewModel: BaseViewModel
    { 
		
        [DisplayName("UserId")]
		public Guid UserId { get; set; }
		
        [DisplayName("Name")]
		public string? Name { get; set; }
		
        [DisplayName("Family")]
		public string? Family { get; set; }
		
        [DisplayName("NationalCode")]
		public string? NationalCode { get; set; }
		
        [DisplayName("PhoneNumber")]
		public string? PhoneNumber { get; set; }
		
        [DisplayName("Email")]
		public string? Email { get; set; }
	}
}