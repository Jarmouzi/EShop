using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class StateViewModel: BaseViewModel
    { 
		
        [DisplayName("Title")]
		public string? Title { get; set; }
	}
}