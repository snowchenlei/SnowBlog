﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Snow.Blog.Web.Areas.Manager.Controllers
{
    public class AccountController : ManagerController
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}