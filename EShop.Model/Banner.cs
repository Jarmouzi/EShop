using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Banner: BaseModel
    { 
		public string? Title { get; set; }
		public string? Image { get; set; }
		public string? Path { get; set; }
		public bool? Confirmed { get; set; }
	}
}