using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Collection: BaseModel
    { 
		public string? CN { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Search { get; set; }
		public int? Count { get; set; }
		public bool? IsMain { get; set; }
	}
}