using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class GroupType: BaseModel
    { 
		public string? Title { get; set; }
		public string? Image { get; set; }
		public Int16? MaxCount { get; set; }
		public bool? Confirmed { get; set; }
	}
}