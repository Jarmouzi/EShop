using EShop.Model;
using EShop.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModel
{
    public class CartViewModel: BaseViewModel
    {
        public CartViewModel()
        {
            Items = new List<CartItem>();
        }


        [Required]
        [MaxLength(12)]
        [DisplayName("Handle")]
        public string Handle { get; set; } = ExtentionMethod.GenerateUniqueCode();
        		
        [DisplayName("Amount")]
		public long Amount { get; set; } = 0;

        [DisplayName("TaxAmount")]
		public long TaxAmount { get; set; } = 0;

        [DisplayName("DiscountAmount")]
		public long DiscountAmount { get; set; } = 0;

        [DisplayName("TotalAmount")]
		public long TotalAmount { get; set; } = 0;

        [DisplayName("DiscountCode")]
		public string? DiscountCode { get; set; }

        [DisplayName("Quantity")]
        public int Quantity { get; set; } = 0;

        [DisplayName("Items")]
        public IEnumerable<CartItem> Items { get; set; }
	}
}