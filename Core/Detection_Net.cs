using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core
{
    public class Detection_Net
    {
        //去掉网址前缀
        static string RemoveText(string url)
        {
            if (url.IndexOf("https") >= 0)
            {
                return url.Replace("https://", "");
            }
            else
            {
                string adcc = url.Replace("http://", "");
                string addr = adcc.Substring(0, 13);
                return addr;

            }
        }
        //主网络服务器数据获取,t1为主服务器,t2为辅助服务器.比较延迟取优
        public static void Tese_deping()
        {
            string surl_github = "github.com";
            string surl_zh_cn = "coding.net";
            string glurl;
            if (RRT(surl_github) <= RRT(surl_zh_cn))
            {
                glurl = Properties.Resources.Github_Url;
            }
            else
            {
                glurl = Properties.Resources.Coding_Url;
            }
            RTUrl(glurl);
        }

        static long RRT(string url)
        {
            System.Net.NetworkInformation.Ping p1 = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply reply = p1.Send(url);
            System.Text.StringBuilder sbuilder;
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                sbuilder = new System.Text.StringBuilder();
                sbuilder.Append(string.Format("RoundTrip time: {0} ", reply.RoundtripTime));
            }
            else if (reply.Status == System.Net.NetworkInformation.IPStatus.TimedOut)
            {
                MessageBox.Show("主数据服务器连接超时", "网络提示");
            }
            else
            {
                MessageBox.Show("主数据服务器连接失败", "网络提示");
            }

            return reply.RoundtripTime;
        }
        public static JObject RTSUrl;
        public static bool RTUrl(string url)
        {
            if (Core.GetUserData.Get_Language_Data(url) == true)
            {
                RTSUrl = Core.GetUserData._Language_Data;
                return true;
            }else
            {
                return false;
            }
            
        }
    }
}
