using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Region: BaseModel
    { 
		public string? Title { get; set; }
		public string? Country { get; set; }
	}
}