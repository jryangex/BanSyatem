using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ReadUrlText
    {
        public static  string Get(string strURL)
        {
            HttpWebRequest request;
            // 创建一个HTTP请求
            request = (HttpWebRequest)WebRequest.Create(strURL);
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;
        }
    }
}
