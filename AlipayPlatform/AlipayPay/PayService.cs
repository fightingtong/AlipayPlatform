using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlipayPlatform.AlipayPay
{
    public class PayService
    {
        /// <summary>
        /// 支付宝原生APi无密支付：建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <returns>支付宝处理结果</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp)
        {
            var code = Encoding.GetEncoding(Env.InputCharset);

            // 待请求参数数组字符串.
            var strRequestData = BuildRequestParaToString(sParaTemp, code);

            // 把数组转换成流中所需字节数组类型.
            var bytesRequestData = code.GetBytes(strRequestData);

            // 构造请求地址.
            var strUrl = Env.GatewayNew + "_input_charset=" + Env.InputCharset;

            // 请求远程HTTP.
            var strResult = "";
            try
            {
                // 设置HttpWebRequest基本信息.
                var myReq = (HttpWebRequest)WebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                // 填充POST数据.
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                // 发送POST数据请求服务器.
                var httpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = httpWResp.GetResponseStream();

                // 获取服务器返回信息.
                if (myStream != null)
                {
                    var reader = new StreamReader(myStream, code);
                    var responseData = new StringBuilder();
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        responseData.Append(line);
                    }

                    // 释放.
                    myStream.Close();

                    strResult = responseData.ToString();
                }
            }
            catch (Exception exp)
            {
                strResult = "报错：" + exp.Message;
            }

            return strResult;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        public static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            // 待签名请求参数数组.
            var sPara = BuildRequestPara(sParaTemp);

            // 把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode.
            var strRequestData = StringHelper.CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            // 过滤签名参数数组.
            var sPara = StringHelper.FilterPara(sParaTemp);

            // 获得签名结果.
            var mysign = BuildRequestMysign(sPara);

            // 签名结果与签名方式加入请求提交参数组中.
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", Env.SignType);

            return sPara;
        }

        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给支付宝的参数数组</param>
        /// <returns>签名结果</returns>
        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            // 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串.
            var prestr = StringHelper.CreateLinkString(sPara);

            // 把最终的字符串签名，获得签名结果.
            string mysign;
            switch (Env.SignType)
            {
                case "MD5":
                    mysign = AliPayMd5.Sign(prestr, Env.Secret, Env.InputCharset);
                    break;
                default:
                    mysign = "";
                    break;
            }

            return mysign;
        }
    }
}
