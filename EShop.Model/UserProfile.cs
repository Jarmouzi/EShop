using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class UserProfile: BaseModel
    { 
		public Guid UserId { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? Family { get; set; }
		[Column(TypeName = "char(10)")]
		public string? NationalCode { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? PhoneNumber { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? Email { get; set; }
	}
}