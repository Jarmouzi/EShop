using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Product_Image: BaseModel
    { 
		public Int64 ProductId { get; set; }
		public Int64? Product_OptionId { get; set; }
		public Int64? ImageId { get; set; }
		public bool? Featured { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

		[ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

		[ForeignKey("Product_OptionId")]
        public virtual Product_Option Product_Option { get; set; }
	}
}