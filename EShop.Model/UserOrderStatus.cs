using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class UserOrderStatus: BaseModel
    { 
		[Column(TypeName = "nvarchar(100)")]
		public string? Title { get; set; }
		[Column(TypeName = "nvarchar(100)")]
		public string? ClassName { get; set; }
				public virtual ICollection<UserOrder> UserOrders { get; set; }
	}
}