using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Model
{
    public class Product : BaseModel
    {
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public bool Confirmed { get; set; }
    }
}
