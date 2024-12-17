using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Cart: BaseModel
    { 
		[Column(TypeName = "char(12)")]
        public string Handle { get; set; }
		public Int64 Amount { get; set; } = 0;
		public Int64 TaxAmount { get; set; } = 0;
        public Int64 DiscountAmount { get; set; } = 0;
        public Int64 TotalAmount { get; set; } = 0;

        [Column(TypeName = "nvarchar(200)")]
		public string? DiscountCode { get; set; }
		public int Quantity { get; set; } = 0;
        public virtual ICollection<CartItem> CartItems { get; set; }
	}
}