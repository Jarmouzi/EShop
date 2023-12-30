using EShop.Utilities;

namespace EShop.ViewModel
{
    public class SaleTypeViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public int? InstallmentCount { get; set; }
		public int? Profit { get; set; }
		public bool? Confirmed { get; set; }
	}
}