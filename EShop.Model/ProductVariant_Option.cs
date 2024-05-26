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
		public Int64? OptionId { get; set; }
		public Int64? OptionValueId { get; set; }

		[ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

		[ForeignKey("OptionId")]
        public virtual Option Option { get; set; }

		[ForeignKey("OptionValueId")]
        public virtual OptionValue OptionValue { get; set; }
	}
}