using Cl.Blog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.DAL
{
    public class BaseDal
    {
        protected readonly DbOperation CurrentDbOperation = null;
        public BaseDal()
        {
            CurrentDbOperation = DbOperation.GetDbOperation("sqlserverConn");
        }
    }
}
