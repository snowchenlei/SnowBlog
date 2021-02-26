using Snow.Blog.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Snow.Blog
{
    [DependsOn(
        typeof(BlogEntityFrameworkCoreTestModule)
        )]
    public class BlogDomainTestModule : AbpModule
    {

    }
}