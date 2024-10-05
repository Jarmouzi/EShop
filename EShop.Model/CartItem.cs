using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class CartItem: BaseModel
    { 
		public Int64? CartId { get; set; }
		public Int64? Amount { get; set; }
		public Int64? DiscountAmount { get; set; }
		public int? Quantity { get; set; }
		public Int64? ProductVarientId { get; set; }
		[Column(TypeName = "char(12)")]
		public string? ProductHandle { get; set; }
		[Column(TypeName = "nvarchar(2000)")]
		public string? ProductUrl { get; set; }

		[ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
	}
}