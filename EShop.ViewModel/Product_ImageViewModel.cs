using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Product_ImageViewModel: BaseViewModel
    { 
		public long ProductId { get; set; }
		public long? ProductVariantId { get; set; }
		public long? ImageId { get; set; }
		public bool? Featured { get; set; }
		public bool? Confirmed { get; set; }
		public string ProductTitle { get; set; }
		public string ImageTitle { get; set; }
		public string ProductVariantTitle { get; set; }
	}
}