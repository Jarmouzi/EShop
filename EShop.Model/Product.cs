using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Product: BaseModel
    { 
		public string? Title { get; set; }
		public Int64? CategoryId { get; set; }
		public Int64? BrandId { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

		[ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
				public virtual ICollection<Product_Feature> Product_Features { get; set; }
				public virtual ICollection<ProductInPage> ProductInPages { get; set; }
	}
}