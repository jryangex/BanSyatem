using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Threading;

namespace Core
{

    public class GetUserData
    {     
        //外服全局获取数据操作
        public static string ship_data_p;
        //数据获取
        public static bool Get_data_x(string url)
        {
            try
            {
                HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse hwRes = (HttpWebResponse)hwReq.GetResponse();
                hwReq.Method = "Get";
                hwReq.Timeout = 180000;
                hwReq.KeepAlive = false;
                //从输入的网站提取HTML源码
                StreamReader reader = new StreamReader(hwRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                ship_data_p = reader.ReadToEnd();
                System.GC.Collect();
                return true;
            }
            catch
            {
                return false;
            }
           
        }
        //国服全局获取数据操作
        public static dynamic wtr_use_data_p;
        //数据获取
        public static bool Wtr_Get_data_x(string url)
        {
            try
            {
                HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse hwRes = (HttpWebResponse)hwReq.GetResponse();
                hwReq.Method = "Get";
                hwReq.Timeout = 180000;
                hwReq.KeepAlive = false;
                //从输入的网站提取HTML源码
                StreamReader reader = new StreamReader(hwRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                wtr_use_data_p = JsonConvert.DeserializeObject(reader.ReadToEnd().ToString());
                System.GC.Collect();
                return true;
            }
            catch
            {
                return false;
            }
          
          
        }
        //公共JObject列表获取:
        public static JObject _Language_Data;
        public static bool Get_Language_Data(string url)
        {
            try
            {
                HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse hwRes = (HttpWebResponse)hwReq.GetResponse();
                hwReq.Method = "Get";
                hwReq.Timeout = 180000;
                hwReq.KeepAlive = false;
                //从输入的网站提取HTML源码
                StreamReader reader = new StreamReader(hwRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                _Language_Data = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());
                System.GC.Collect();
                return true;
            }
            catch
            {
                return false;
            }
           
        }
    }
}
