using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Model
{
    public class Image : BaseModel
    {
        public string? Url { get; set; }
        public string? AltText { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool? Confirmed { get; set; }
        public virtual ICollection<OptionValue> OptionValues { get; set; }
        public virtual ICollection<Product_Image> Product_Images { get; set; }
    }
}