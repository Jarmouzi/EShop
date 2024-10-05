using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class UserAddressViewModel: BaseViewModel
    { 
		
        [DisplayName("UserId")]
		public Guid? UserId { get; set; }
		
        [DisplayName("StateId")]
		public long? StateId { get; set; }
		
        [DisplayName("CityId")]
		public long? CityId { get; set; }
		
        [DisplayName("Address")]
		public string? Address { get; set; }
	}
}