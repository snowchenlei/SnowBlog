using System;
using System.ComponentModel.DataAnnotations;
using Snow.Blog.Model.Enum;

namespace Snow.Blog.Service.Dto
{
    public class PagedAndSortedInputDto
    {
        public PagedAndSortedInputDto()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "ID";
            }
        }

        public string Sorting { get; set; }

        public OrderType Order { get; set; }

        /// <summary>
        /// 跳过多少条
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        [Range(1, 500)]
        public int Limit { get; set; }
    }
}