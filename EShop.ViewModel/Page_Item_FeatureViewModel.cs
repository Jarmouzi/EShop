using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Page_Item_FeatureViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? ComponentName { get; set; }
		public Int64? Page_Item_Id { get; set; }
		public bool? Confirmed { get; set; }
	}
}