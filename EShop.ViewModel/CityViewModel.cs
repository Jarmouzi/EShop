using EShop.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class CityViewModel: BaseViewModel
    { 
		
        [DisplayName("StateId")]
		public long? StateId { get; set; }
		
        [DisplayName("Title")]
		public string? Title { get; set; }

  //      [ValidateNever]
		//public string StateTitle { get; set; }
	}
}