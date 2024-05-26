using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Product_Feature_Detail: BaseModel
    { 
		public string? Title { get; set; }
		public string? Detail { get; set; }
		public string? Image { get; set; }
		public Int64? Product_FeatureId { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("Product_FeatureId")]
        public virtual Product_Feature Product_Feature { get; set; }
	}
}