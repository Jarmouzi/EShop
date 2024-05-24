using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Item_Feature_Detail: BaseModel
    { 
		public string? Title { get; set; }
		public string? Detail { get; set; }
		public string? Image { get; set; }
		public Int64? Item_FeatureId { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("Item_FeatureId")]
        public virtual Product_Feature Product_Feature { get; set; }
	}
}