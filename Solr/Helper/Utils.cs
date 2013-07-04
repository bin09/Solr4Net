using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Solr.Helper
{
    public class Utils
    {
        public static Action<string> DisplayMessage;
        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearHtml(string str)
        {
            string text1 = "<.*?>";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            str = str.Replace("&nbsp;", "");
            return str;
        }
        /// <summary>
        /// 转换字符串数组为整型数组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int[] ConvertStringArrayInIntArrat(string[] ids)
        {
            ArrayList list = new ArrayList();
            foreach (string id in ids)
            {
                try
                {
                    list.Add(int.Parse(id));
                }
                catch
                {
                    list.Add(0);
                }
            }
            return (int[])list.ToArray(typeof(int));
        }
    }
}
