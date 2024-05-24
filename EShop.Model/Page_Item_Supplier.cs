using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Page_Item_Supplier: BaseModel
    { 
		public int? RemainedCount { get; set; }
		public Int64? SupplierId { get; set; }
		public long? Price { get; set; }
		public Int16? Discount { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
	}
}