using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.Blog.Web.Models.Home
{
    public class IndexModel : PageModel
    {
        public Dictionary<int, string> Categories { get; set; }
    }
}