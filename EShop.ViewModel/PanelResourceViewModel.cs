using EShop.Utilities;

namespace EShop.ViewModel
{
    public class PanelResourceViewModel: BaseViewModel
    { 
		public string? PageAddress { get; set; }
		public string? Method { get; set; }
		public string Title { get; set; }
		public string? Icon { get; set; }
		public int? Order { get; set; }
		public bool ShowOnMenu { get; set; }
		public Guid? ParentId { get; set; }
		public bool? Confirmed { get; set; }
	}
}