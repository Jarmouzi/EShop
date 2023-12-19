using EShop.Model.TypeSafe;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModel
{
    public class RegionViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Country { get; set; }
    }

    public class RegionListViewModel {
        public RegionListViewModel()
        {
            Regions = new List<RegionViewModel>();
            TotalCount = 0;
            Take = 10;
            Skip = 0;
        }
        public List<RegionViewModel> Regions { get; set; }

        public SelectList PaginationList { get { return new SelectList(TS.DefaultValue.Pagination, 10); } }
        public int TotalCount { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
