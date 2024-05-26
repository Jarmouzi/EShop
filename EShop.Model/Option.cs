using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Option: BaseModel
    { 
		public string? Title { get; set; }
				public virtual ICollection<ProductVariant_Option> ProductVariant_Options { get; set; }
	}
}