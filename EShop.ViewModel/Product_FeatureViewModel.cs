using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Product_FeatureViewModel: BaseViewModel
    {
        public Int64? ProductId { get; set; }
        public Int64? FeatureId { get; set; }
        public string? Value { get; set; }
		public int? DisplayOrder { get; set; }
		public bool? Confirmed { get; set; }
		public string FeatureTitle { get; set; }
		public string ProductTitle { get; set; }
	}
}