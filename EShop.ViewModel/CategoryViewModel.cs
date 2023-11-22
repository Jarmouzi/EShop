using EShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModel
{
    public class CategoryViewModel : BaseModel
    {
        public string Title { get; set; }
        public short Level { get; set; }
        public Guid ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool Confirmed { get; set; }

        public string ParentTitle { get; set; }
    }
}
