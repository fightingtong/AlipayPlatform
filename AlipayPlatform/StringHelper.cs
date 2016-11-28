using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AlipayPlatform
{
    public class StringHelper
    {
        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            var prestr = new StringBuilder();
            foreach (var temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            // 去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            var dicArray = new Dictionary<string, string>();
            foreach (var temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            var prestr = new StringBuilder();
            foreach (var temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            // 去掉最後一個&字符
            var nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
    }
}
