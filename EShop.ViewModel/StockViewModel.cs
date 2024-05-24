using EShop.Utilities;

namespace EShop.ViewModel
{
    public class StockViewModel: BaseViewModel
    { 
		public Int64? ItemId { get; set; }
		public long? Price { get; set; }
		public DateTime? Date { get; set; }
		public double? Count { get; set; }
		public string? TranType { get; set; }
		public string? Desc { get; set; }
		public Int64? Item_FeatureId1 { get; set; }
		public Int64? Item_FeatureId2 { get; set; }
		public Int64? Item_FeatureId3 { get; set; }
		public Int64? InventoryId { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }
		public string DateStr 
		{ 
			get
			{
                return Date.ToCultureDate();
			}
		}
		
	}
}