using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class OptionValue: BaseModel
    { 
		public string? Title { get; set; }
		public Int64? ImageId { get; set; }
		public string? Color { get; set; }

		[ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
				public virtual ICollection<ProductVariant_Option> ProductVariant_Options { get; set; }
	}
}