using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AlipayPlatform.Api
{
    /// <summary>
    /// 原生支付宝回调Api
    /// </summary>
    public class batch_trans_notify_no_pwd : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Pragma", "No-Cache");
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";

            BindData(context);
        }

        private void BindData(HttpContext context)
        {
            var url = context.Request.Url;
            //通知时间.
            var notify_time = context.Request["notify_time"];

            //通知类型.
            var notify_type = context.Request["notify_type"];

            //通知校验ID.
            var notify_id = context.Request["notify_id"];

            //签名方式.
            var sign_type = context.Request["sign_type"];

            //签名.
            var sign = context.Request["sign"];

            //批次号.
            var batch_no = context.Request["batch_no"];

            //付款账号ID.
            var pay_user_id = context.Request["pay_user_id"];

            //付款账号姓名.
            var pay_user_name = context.Request["pay_user_name"];

            //付款账号.
            var pay_account_no = context.Request["pay_account_no"];

            //转账失败的详细信息  40^15080318813^林雪萍1^0.92^F^ACCOUN_NAME_NOT_MATCH^20150731524125884^20150731150015|
            var fail_details = context.Request["fail_details"];

            //转账成功的详细信息  35^18960107451^杨彩燕^0.92^S^^20150731524125883^20150731150015|
            var success_details = context.Request["success_details"];

            if (string.IsNullOrEmpty(notify_id))
                return;

            var content = VerifyAlipaySource(notify_id);
            if (content.ToLower() != "true")
                return;

            // 进行更新支付状态处理。
            FailDetails(fail_details, batch_no);

            SuccessDetails(success_details, batch_no);

            context.Response.Write(1);
        }

        /// <summary>
        /// 校验notify_id判断是否来及支付宝的异步回调。
        /// </summary>
        /// <param name="notify_id">通知校验ID.</param>
        /// <returns></returns>
        private static string VerifyAlipaySource(string notify_id)
        {
            var url = string.Format("{0}service=notify_verify&partner={1}&notify_id={2}"
                , Env.GatewayNew, Env.Partner, notify_id);

            var content = Encoding.UTF8.GetString(new WebClient().DownloadData(url));

            return content;
        }

        /// <summary>
        /// 拿到支付失败明细批量更新提现记录。
        /// </summary>
        /// <param name="fail_details"></param>
        /// <param name="batch_no"></param>
        private void FailDetails(string fail_details, string batch_no)
        {
            // 转账失败的.
            if (string.IsNullOrEmpty(batch_no) || string.IsNullOrEmpty(fail_details))
                return;

            var fail_arr = fail_details.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (fail_arr.Length <= 0)
                return;

            foreach (var fail in fail_arr)
            {
                var info = fail.Split(new[] { "^" }, StringSplitOptions.None);
                if (info.Length < 6 || string.IsNullOrEmpty(info[0]))
                    continue;

                var id = 0;
                int.TryParse(info[0], out id);

                // 此处省略：根据batch_no找到对应的支付宝提现请求记录，进行状态更新。
            }
        }

        /// <summary>
        /// 拿到支付成功明细批量更新提现记录。
        /// </summary>
        /// <param name="success_details"></param>
        /// <param name="batch_no"></param>
        private void SuccessDetails(string success_details, string batch_no)
        {
            // 转账成功的.
            if (string.IsNullOrEmpty(batch_no) || string.IsNullOrEmpty(success_details))
                return;

            var success_arr = success_details.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (success_arr.Length <= 0)
                return;

            foreach (var success in success_arr)
            {
                var info = success.Split(new string[] { "^" }, StringSplitOptions.None);
                if (info.Length < 6 || string.IsNullOrEmpty(info[0]))
                    continue;

                var id = 0;
                int.TryParse(info[0], out id);
                
                // 此处省略：根据batch_no找到对应的支付宝提现请求记录，进行状态更新。
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}