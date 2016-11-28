using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlipayPlatform
{
    public class Env
    {
        // 支付宝网关地址（新）
        public static string GatewayNew = "https://mapi.alipay.com/gateway.do?";

        public static string Partner = "1234567890123456";

        // 商户的私钥
        public static string Secret = "xxxxxxxxxxxxxxx";

        // 编码格式
        public static string InputCharset = "utf-8";

        // 签名方式
        public static string SignType = "MD5";


        // 服务器异步通知页面路径【需http://格式的完整路径，不允许加?id=123这类自定义参数】.
        public static string NotifyUrl = "http://xxx.yyy.com/batch_trans_notify_no_pwd.ashx";

        // 付款账号【必填】.
        public static string Email = "xxx@yyy.com";

        // 付款账户名【必填，个人支付宝账号是真实姓名,公司支付宝账号是公司名称】.
        public static string AccountName = "xxx公司";
    }
}
