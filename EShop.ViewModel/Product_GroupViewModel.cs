using EShop.Utilities;

namespace EShop.ViewModel
{
    public class Product_GroupViewModel: BaseViewModel
    { 
		public Guid? ProductId { get; set; }
		public Guid? SimilarProductId { get; set; }
		public Guid? GroupId { get; set; }
		public bool? Confirmed { get; set; }
	}
}