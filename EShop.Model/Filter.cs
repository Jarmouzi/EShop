using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Filter: BaseModel
    { 
		public Guid? FeatureId { get; set; }
		public Guid? CategoryId { get; set; }
		public string? ComponentName { get; set; }
		public bool? Confirmed { get; set; }
		public string? Title { get; set; }

		[ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

		[ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }
	}
}