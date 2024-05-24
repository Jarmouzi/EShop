using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class PanelResource: BaseModel
    { 
		public string? PageAddress { get; set; }
		public string? Method { get; set; }
		public string Title { get; set; }
		public string? Icon { get; set; }
		public int? SortOrder { get; set; }
		public bool ShowOnMenu { get; set; }
		public Int64? ParentId { get; set; }
		//public bool? Confirmed { get; set; }
	}
}