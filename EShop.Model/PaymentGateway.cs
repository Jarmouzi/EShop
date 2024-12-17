using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class PaymentGateway: BaseModel
    { 
		[Column(TypeName = "nvarchar(400)")]
		public string? Title { get; set; }
		[Column(TypeName = "nvarchar(1000)")]
		public string? Description { get; set; }
		[Column(TypeName = "nvarchar(2000)")]
		public string? Url { get; set; }
		public bool? IsPublic { get; set; }
	}
}