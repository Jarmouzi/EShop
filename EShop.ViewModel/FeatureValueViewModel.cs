using EShop.Utilities;
using System.ComponentModel;

namespace EShop.ViewModel
{
    public class FeatureValueViewModel: BaseViewModel
    { 
		
        [DisplayName("FeatureId")]
		public long? FeatureId { get; set; }
		
        [DisplayName("Value")]
		public string? Value { get; set; }
		public string FeatureTitle { get; set; }
	}
}