namespace Core
{
    public class GetUrl
    {
        public static GetUrlData url = new GetUrlData();
        public class GetUrlData //非国服返回类本类返回数据类
        {
            public string API_Server_Url { get; set; } 
        }
        //增加服务器到控件
        public static string Function_Server_list_name(int i)
        {
            string[] Server_List = { language .lng_urld.asia, language.lng_urld.eu,language.lng_urld.com, language.lng_urld.ru, language.lng_urld.CN_S, language.lng_urld.CN_N };
            return Server_List[i];
        }
        //获取用户选择的服务器对应区域
        private static string Function_Server_list_coe(int SelectedIndex)
        {
            string server_coe="";
            switch (SelectedIndex)
            {
                case 0:
                    server_coe = "asia";
                    break;
                case 1:
                    server_coe = "eu";
                    break;
                case 2:
                    server_coe = "na";
                    break;
                case 3:
                    server_coe = "ru";
                    break;
                case 4:
                    server_coe = "cns";
                    break;
                case 5:
                    server_coe = "cnn";
                    break;
            }
            return server_coe;
        }
      
        //拟合网址并返回数据类
        public static GetUrlData Get_server_url(int selectedindex)
        {
            url.API_Server_Url = Properties.Resources.Server_Player_Haad_url + Function_Server_list_coe(selectedindex) + Properties.Resources.Server_Player_Over_url;
            return url;
        }
        ///////////////////////////////////////////////////////////Url完毕/////////////////////////////////
    }

}
