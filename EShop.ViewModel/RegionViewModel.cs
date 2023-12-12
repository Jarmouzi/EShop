using EShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModel
{
    public class RegionViewModel : BaseModel
    {
        public string Title { get; set; }
        public string Country { get; set; }
    }

    public class RegionListViewModel {
        public List<RegionViewModel> Regions { get; set; }
        public int TotalCount { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
