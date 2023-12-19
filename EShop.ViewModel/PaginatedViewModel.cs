using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModel
{
    public class PaginatedViewModel<T> where T : class
    {
        public PaginatedViewModel()
        {
            Data = new List<T>();
        }
        public IEnumerable<T> Data { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
