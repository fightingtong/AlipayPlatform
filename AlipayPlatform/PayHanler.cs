using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AlipayPlatform.AlipayPay;

namespace AlipayPlatform
{
    public class PayHanler
    {
        /// <summary>
        /// 【支付宝】账号支付。
        /// </summary>
        /// <param name="batchFee">付款总金额【必填，即参数detail_data的值中所有金额的总和】</param>
        /// <param name="batchNum">付款笔数【必填，即参数detail_data的值中，“|”字符出现的数量加1，最大支持1000笔（即“|”字符出现的数量999个）】</param>
        /// <param name="detailData">付款详细数据【必填，格式：流水号1^收款方账号1^真实姓名^付款金额1^备注说明1|流水号2^收款方账号2^真实姓名^付款金额2^备注说明2....】</param>
        /// <returns></returns>
        public int AliPayAccountPay(string batchFee, string batchNum, string detailData)
        {
            #region

            // 付款当天日期【必填，格式：年[4位]月[2位]日[2位]，如：20100801】.
            var payDate = DateTime.Now.ToString("yyyyMMdd");

            // 批次号【必填，格式：当天日期[8位]+序列号[3至16位]，如：201008010000001】.
            var batchNo = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyMMdd")
                , DateTime.Now.ToString("HHmmss"), new Random().Next(1000, 9999));

            #endregion
            ////////////////////////////////////////////////////////////////////////////////////////////////

            // 把请求参数打包成数组
            var sParaTemp = new SortedDictionary<string, string>
            {
                {"partner", Env.Partner},
                {"_input_charset", Env.InputCharset.ToLower()},
                {"service", "batch_trans_notify_no_pwd"},
                {"notify_url", Env.NotifyUrl},
                {"email", Env.Email},
                {"account_name", Env.AccountName},
                {"pay_date", payDate},
                {"batch_no", batchNo},
                {"batch_fee", batchFee},
                {"batch_num", batchNum},
                {"detail_data", detailData}
            };

            // 建立请求
            var content = PayService.BuildRequest(sParaTemp);
            if (string.IsNullOrEmpty(content))
                return 0;

            var doc = new XmlDocument();
            doc.LoadXml(content);
            var xn = doc.SelectSingleNode("alipay");
            if (xn == null)
                return 0;

            var isSuccess = xn.SelectSingleNode("is_success");
            var txtIsSuccess = isSuccess == null
                ? "" : isSuccess.InnerText.ToUpper();

            var error = xn.SelectSingleNode("error");
            var txtError = error == null
                ? "" : error.InnerText.ToUpper();

            if (string.IsNullOrEmpty(txtIsSuccess) || string.IsNullOrEmpty(txtError))
                return 0;
            
            // 此处省略：可以根据支付宝支付请求返回结果，记录请求记录，更新提现申请批次号（提现结果状态需等待支付宝回调接口处理）
            return 0;
        }
    }
}
