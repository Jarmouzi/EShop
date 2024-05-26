using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ProductSeoViewModel: BaseViewModel
    { 
		public long? ProductVariantId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public bool? IsName { get; set; }
		public string ProductVariantTitle { get; set; }
	}
}