using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class UserOrder: BaseModel
    { 
		[Column(TypeName = "char(12)")]
		public string? Handle { get; set; }
		public Guid? UserId { get; set; }
		public Int64? CartId { get; set; }
		public Int64? UserAddressId { get; set; }
		public Int64? UserOrderStatusId { get; set; }
		public Int64? PaymentGatewayId { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? PaymentConfirmationCode { get; set; }

		[ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }

		[ForeignKey("UserAddressId")]
        public virtual UserAddress UserAddress { get; set; }

		[ForeignKey("UserOrderStatusId")]
        public virtual UserOrderStatus UserOrderStatus { get; set; }
	}
}