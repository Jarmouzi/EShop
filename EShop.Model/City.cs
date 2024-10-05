using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class City: BaseModel
    { 
		public Int64? StateId { get; set; }
		[Column(TypeName = "nvarchar(200)")]
		public string? Title { get; set; }

		[ForeignKey("StateId")]
        public virtual State State { get; set; }
	}
}