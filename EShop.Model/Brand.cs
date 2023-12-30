using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Brand: BaseModel
    { 
		public string? Title { get; set; }
		public string? ThemeAndOtherOptions { get; set; }
		public bool? Confirmed { get; set; }
				public virtual ICollection<Product> Products { get; set; }
	}
}