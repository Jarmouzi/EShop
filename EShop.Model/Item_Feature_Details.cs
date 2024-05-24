using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Item_Feature_Details: BaseModel
    { 
		public Int64? Item_FeatureId { get; set; }
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? details { get; set; }
		public bool? Confirmed { get; set; }

		[ForeignKey("Item_FeatureId")]
        public virtual Page_Item_Feature Page_Item_Feature { get; set; }
	}
}