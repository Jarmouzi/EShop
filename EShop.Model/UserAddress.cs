using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class UserAddress: BaseModel
    { 
		public Guid? UserId { get; set; }
		public Int64? StateId { get; set; }
		public Int64? CityId { get; set; }
		[Column(TypeName = "nvarchar(max)")]
		public string? Address { get; set; }
	}
}