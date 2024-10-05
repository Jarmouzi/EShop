using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class State: BaseModel
    { 
		[Column(TypeName = "nvarchar(200)")]
		public string? Title { get; set; }
				public virtual ICollection<City> Citys { get; set; }
	}
}