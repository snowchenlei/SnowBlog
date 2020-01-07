using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.Common
{
    public class DataTableHelper
    {
        /// <summary>
        /// DataTable转模型
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="table">表</param>
        /// <returns>包含数据的模型</returns>
        public static T DataTableToModel<T>(DataTable table) where T : class, new()
        {
            if (table.Rows.Count > 1)
            {
                throw new ArgumentException("table只允许包含一条数据");
            }
            T entity = new T();
            PropertyInfo[] propertyInfos = entity.GetType().GetProperties();
            string pName = string.Empty;
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                pName = propertyInfo.Name;
                //如果列名与属性名相同则赋值
                if (table.Columns.Contains(pName))
                {
                    object value = table.Rows[0][pName];
                    propertyInfo.SetValue(pName, value);
                }
            }
            return entity;
        }

        /// <summary>
        /// DataTable转集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="table">数据集合</param>
        /// <returns>实体类集合</returns>
        public static IEnumerable<T> DataTableToList<T>(DataTable table) where T : class, new()
        {
            List<T> entities = new List<T>();
            foreach (DataRow dr in table.Rows)
            {
                T entity = Activator.CreateInstance<T>();
                PropertyInfo[] propertyInfos = entity.GetType().GetProperties();
                string pName = string.Empty;
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    pName = propertyInfo.Name;
                    //如果列名与属性名相同则赋值
                    if (table.Columns.Contains(pName))
                    {
                        object value = dr[pName];
                        if (value != DBNull.Value)
                        {
                            if (propertyInfo.PropertyType.FullName == "System.Boolean" && !(value is bool))
                            {
                                propertyInfo.SetValue(entity, value.ToString() == "1" ? true : false);
                            }
                            else
                            {
                                propertyInfo.SetValue(entity, value);
                            }
                        }
                    }
                }
                yield return entity;
            }
        }
    }
}
