using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cl.Blog.Common
{
    public class ReflectHelper
    {
        /// <summary>
        /// 获取枚举的所有值、描述信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumDesc<T>()where T:struct
        {            
            
            FieldInfo[] fields = typeof(T).GetFields();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (FieldInfo filed in fields)
            {
                 object[] attributes = filed.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if(attributes.Length == 1)
                {
                    dic.Add(filed.Name ,((System.ComponentModel.DescriptionAttribute)attributes[0]).Description);
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取枚举某一值的描述信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filedName">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescByFieldName<T>(string filedName)
        {
            FieldInfo field = typeof(T).GetField(filedName);
            object[] attributes = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (attributes.Length == 1)
            {
                return ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
            }
            return "";
        }
    }
}
