using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class ProductVariant: BaseModel
    { 
		public Int64 ProductId { get; set; }
		public Int64? SupplierId { get; set; }
		public bool? AvailableForSale { get; set; }
		public Int64? Price { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

		[ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
				public virtual ICollection<ProductSeo> ProductSeos { get; set; }
				public virtual ICollection<ProductVariant_Option> ProductVariant_Options { get; set; }
				public virtual ICollection<Product_Image> Product_Images { get; set; }
	}
}