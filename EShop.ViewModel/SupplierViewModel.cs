using EShop.Utilities;

namespace EShop.ViewModel
{
    public class SupplierViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public Int64? OwnerId { get; set; }
		public string? Address { get; set; }
		public string? PhoneNumber { get; set; }
		public string? FaxNumber { get; set; }
		public string? Logo { get; set; }
		public string? Banner { get; set; }
        public string? Signe { get; set; }
        public bool? Confirmed { get; set; }
	}
}