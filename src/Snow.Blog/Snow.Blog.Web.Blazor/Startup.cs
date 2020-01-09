using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snow.Blog.IDAL.Categories;
using Snow.Blog.IRepository.Bloggers;
using Snow.Blog.Service.Bloggers;
using Snow.Blog.Service.Categories;
using Snow.Blog.SqlServer;
using Snow.Blog.Web.Blazor.Data;

namespace Snow.Blog.Web.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            var application = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.Blog.Service"));
            //var web = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.Blog.Web"));
            services.AddAutoMapper(application);
            services.AddTransient<IBloggerService, BloggerService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBloggerRepository, BloggerRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}