using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class FeatureValue: BaseModel
    { 
		public Int64? FeatureId { get; set; }
		public string? Value { get; set; }

		[ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }
	}
}