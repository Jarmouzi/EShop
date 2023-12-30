using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Supplier: BaseModel
    { 
		public string? Title { get; set; }
		public Guid? OwnerId { get; set; }
		public string? Address { get; set; }
		public string? PhoneNumber { get; set; }
		public string? FaxNumber { get; set; }
		public string? Logo { get; set; }
		public string? Banner { get; set; }
		public bool? Confirmed { get; set; }
				public virtual ICollection<Supplier_Contract> Supplier_Contracts { get; set; }
				public virtual ICollection<ProductInPage> ProductInPages { get; set; }
	}
}