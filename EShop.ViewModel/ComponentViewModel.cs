using EShop.Utilities;

namespace EShop.ViewModel
{
    public class ComponentViewModel: BaseViewModel
    { 
		public Guid? CategoryId { get; set; }
		public string? Title { get; set; }
		public string? Event { get; set; }
		public string? Controller { get; set; }
		public string? Action { get; set; }
		public bool? IsOptional { get; set; }
		public bool? Confirmed { get; set; }
	}
}