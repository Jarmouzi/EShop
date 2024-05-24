using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class ProductInPage: BaseModel
    { 
		public Int64? BrandId { get; set; }
		public Int64? ItemId { get; set; }
		public Int64? Item_FeatureId1 { get; set; }
		public Int64? Item_FeatureId2 { get; set; }
		public Int64? Item_FeatureId3 { get; set; }
		public Int64? SupplierId { get; set; }
		public Int64? Price { get; set; }
		public Int64? DealId { get; set; }
		public int? Discount { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }

		[ForeignKey("ItemId")]
        public virtual Product Product { get; set; }

		//[ForeignKey("Item_FeatureId3")]
  //      public virtual Product_Feature Product_Feature3 { get; set; }

		//[ForeignKey("Item_FeatureId2")]
  //      public virtual Product_Feature Product_Feature2 { get; set; }

		//[ForeignKey("Item_FeatureId1")]
  //      public virtual Product_Feature Product_Feature1 { get; set; }

		[ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
	}
}