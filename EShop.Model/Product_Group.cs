using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Product_Group: BaseModel
    { 
		public Guid? ProductId { get; set; }
		public Guid? SimilarProductId { get; set; }
		public Guid? GroupId { get; set; }
		public bool? Confirmed { get; set; }
	}
}