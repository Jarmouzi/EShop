using EShop.Utilities;

namespace EShop.ViewModel
{
    public class GroupTypeViewModel: BaseViewModel
    { 
		public string? Title { get; set; }
		public string? Image { get; set; }
		public Int16? MaxCount { get; set; }
		public bool? Confirmed { get; set; }
	}
}