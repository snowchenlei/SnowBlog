using System;
using System.Collections.Generic;
using System.Text;
using Snow.Blog.Service.Dto;

namespace Snow.Blog.Service.Bloggers.Dto
{
    public class GetBloggerInput : PagedAndSortedInputDto
    {
        public GetBloggerInput()
        {
            Sorting = "ID";
        }

        public int CategoryId { get; set; }
    }
}