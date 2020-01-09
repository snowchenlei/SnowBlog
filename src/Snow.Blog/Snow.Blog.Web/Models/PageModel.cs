using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.Blog.Web.Models
{
    public class PageModel
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int Limit { get; set; } = 2;

        public int PageCount => TotalCount / Limit + (TotalCount % Limit > 0 ? 1 : 0);
    }
}