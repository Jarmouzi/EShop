using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Component: BaseModel
    { 
		public Int64? CategoryId { get; set; }
		public string? Title { get; set; }
		public string? Event { get; set; }
		public string? Controller { get; set; }
		public string? Action { get; set; }
		public bool? IsOptional { get; set; }
		public bool? Confirmed { get; set; }
	}
}