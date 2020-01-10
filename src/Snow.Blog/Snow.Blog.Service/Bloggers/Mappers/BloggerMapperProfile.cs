using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Snow.Blog.Model.DataBase;
using Snow.Blog.Service.Bloggers.Dto;

namespace Snow.Blog.Service.Bloggers.Mappers
{
    public class BloggerMapperProfile : Profile
    {
        public BloggerMapperProfile()
        {
            CreateMap<Blogger, BloggerListDto>()
                .ForMember(entity => entity.CategoryName,
                    opt => opt.MapFrom(src => (src.Category.Name)))
                .ForMember(entity => entity.LastModifyDate,
                    opt => opt.MapFrom(src => (src.EditDate.ToString("yyyy-MM-dd HH:mm:ss"))));
            CreateMap<Blogger, BloggerDetailDto>()
                .ForMember(entity => entity.CategoryName,
                    opt => opt.MapFrom(src => (src.Category.Name)));
        }
    }
}