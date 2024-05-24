using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Supplier_Contract: BaseModel
    { 
		public Int64? SupplierId { get; set; }
		public Int64? CategoryId { get; set; }
		public Int64? SaleTypeId { get; set; }
		public int? Commission { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }

		[ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

		[ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

		[ForeignKey("SaleTypeId")]
        public virtual SaleType SaleType { get; set; }
	}
}