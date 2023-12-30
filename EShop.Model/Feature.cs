using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Feature: BaseModel
    { 
		public string? Title { get; set; }
		public string? Icon { get; set; }
		public bool? Confirmed { get; set; }
				public virtual ICollection<Filter> Filters { get; set; }
				public virtual ICollection<Product_Feature> Product_Features { get; set; }
	}
}