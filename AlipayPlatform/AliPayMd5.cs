using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlipayPlatform
{
    public class AliPayMd5
    {
        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="inputCharset">编码格式</param>
        /// <returns>签名结果</returns>
        public static string Sign(string prestr, string key, string inputCharset)
        {
            var sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            var t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(prestr));
            for (var i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
    }
}
