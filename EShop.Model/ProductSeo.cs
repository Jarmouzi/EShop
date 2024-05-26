using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class ProductSeo: BaseModel
    { 
		public Int64? ProductVariantId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public bool? IsName { get; set; }

		[ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }
	}
}