using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class SaleType: BaseModel
    { 
		public string? Title { get; set; }
		public int? InstallmentCount { get; set; }
		public int? Profit { get; set; }
		public bool? Confirmed { get; set; }
				public virtual ICollection<Supplier_Contract> Supplier_Contracts { get; set; }
	}
}