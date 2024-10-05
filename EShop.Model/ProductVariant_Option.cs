using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class ProductVariant_Option: BaseModel
    { 
		public Int64? ProductVariantId { get; set; }
		public Int64? Product_OptionId { get; set; }

		[ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

		[ForeignKey("Product_OptionId")]
        public virtual Product_Option Product_Option { get; set; }
	}
}