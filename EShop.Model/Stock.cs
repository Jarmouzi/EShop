using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Stock: BaseModel
    { 
		public Guid? ItemId { get; set; }
		public long? Price { get; set; }
		public DateTime? Date { get; set; }
		public double? Count { get; set; }
		public string? TranType { get; set; }
		public string? Desc { get; set; }
		public Guid? Item_FeatureId1 { get; set; }
		public Guid? Item_FeatureId2 { get; set; }
		public Guid? Item_FeatureId3 { get; set; }
		public Guid? InventoryId { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
	}
}