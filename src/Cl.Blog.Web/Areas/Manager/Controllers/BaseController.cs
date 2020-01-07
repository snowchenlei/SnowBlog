using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Cl.Blog.Web.Areas.Manager.Controllers
{
    public class BaseController:Controller
    {
        public BaseController()
        {
            //if(Session["User"] == null)
            //{
            //    Response.Redirect("/Manager/Login/Index");
            //}
        }
    }
}