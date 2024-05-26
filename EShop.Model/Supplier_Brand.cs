using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Supplier_Brand: BaseModel
    { 
		public Int64? SupplierId { get; set; }
		public Int64? BrandId { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

		[ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
	}
}