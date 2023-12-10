using EShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModel
{
    public class PanelResourceViewModel : BaseModel
    {
        public string PageAddress { get; set; }
        public string Method { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int SortOrder { get; set; }
        public bool ShowOnMenu { get; set; }
        public Guid? ParentId { get; set; }
    }
}
