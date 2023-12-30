using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Page_Item_Feature: BaseModel
    { 
		public string? Title { get; set; }
		public string? ComponentName { get; set; }
		public Guid? Page_Item_Id { get; set; }
		public bool? Confirmed { get; set; }
				public virtual ICollection<Item_Feature_Details> Item_Feature_Detailss { get; set; }
	}
}