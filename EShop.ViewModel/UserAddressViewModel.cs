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
        public string? Title { get; set; }


        [DisplayName("Address")]
		public string? Address { get; set; }

		public string? ReceiverPhoneNumber { get; set; }
		public string? ReceiverName { get; set; }
        public int? Number { get; set; }
        public string? Unit { get; set; }
        public string? PostalCode { get; set; }
        public bool? IsDefault { get; set; }
        public Double? Latitude { get; set; }
        public Double? Longtitude { get; set; }
    }
}