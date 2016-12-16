using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Core
{
    public class language
    {
        public class language_data
        {
            public string BanSystem { get; set; }
            public string Game_Server { get ;set;}
            public string Parsed_Data { get; set; }
            public string Team_One { get; set; }
            public string Tema_Two { get; set; }
            public string asia { get; set; }
            public string eu { get; set; }
            public string com { get; set; }
            public string ru { get; set; }
            public string CN_S { get; set; }
            public string CN_N { get; set; }
            public string DataGrid_Team_Name { get; set; }
            public string DataGrid_Team_Win { get; set; }
            public string DataGrid_Team_Battle { get; set; }
            public string DataGrid_Team_power { get; set; }
            public string Select_Server { get; set; }
            public string Testing_Config { get; set; }
            public string Net_error { get; set; }
            public string Analysis_data { get; set; }
            public string Start_Analysis { get; set; }
            public string Dont_Data_Analysis { get; set; }
            public string BanList_Head_Text { get; set; }
            public string BanTime { get; set; }
            public string BanPlayer_Name { get; set; }
            public string Ban_other { get; set; }
            public string Diy_Ban { get; set; }
            public string Diy_Ban_Time { get; set; }
            public string Diy_Ban_Time_Get { get; set; }
            public string Diy_Ban_Player_Name { get; set; }
            public string Diy_Ban_other { get; set; }
            public string Diy_Ban_Add_Button { get; set; }
            public string Diy_Ban_Del_Button { get; set; }
            public string Diy_Ban_ADD_OK { get; set; }
            public string Diy_Ban_Add_error { get; set; }
            public string Diy_Ban_Del { get; set; }
            public string Diy_Ban_Del_error { get; set; }
            public string Auto_Del_Ban_Name { get; set; }
            public string updata_log_Tetx { get; set; }



        }
        static string[] lng_urldc = new string[] { "language_list_url" , "language_data_url_head" };
        public class language_list
        {
            public string language_name { get; set; }
        }
        public static List<language_list> List_h = new List<language_list>();
        public static bool Getlanguagelist()
        {
            //获取语言列表
            string dst = Core.Detection_Net.RTSUrl[lng_urldc[0]].ToString();
            if (dst != null)
            {
                if (Core.GetUserData.Get_Language_Data(dst) == true)
                {
                    for (int i = 0; i <= GetUserData._Language_Data.Count - 1; i++)
                    {
                        language_list adc = new language_list();
                        adc.language_name = GetUserData._Language_Data["" + i + ""].ToString();
                        List_h.Add(adc);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }else
            {
                return false;
            }
          
               
        }

        public static language_data lng_urld = new language_data ();
        public static bool GetLanguagedata(List<language_list> List_h,int str)
        {
            //获取语言数据 需要做精简优化处理(待处理)
            if(Core.GetUserData.Get_Language_Data(Detection_Net.RTSUrl[lng_urldc[1]] + List_h[str].language_name + ".json") == true)
            {
                JObject LanguageText = GetUserData._Language_Data;
                lng_urld.BanSystem = LanguageText["BanSystem"].ToString();
                lng_urld.Game_Server = LanguageText["Game_Server"].ToString();             
                lng_urld.Parsed_Data = LanguageText["Parsed_Data"].ToString();
                lng_urld.Team_One = LanguageText["Team_One"].ToString();
                lng_urld.Tema_Two = LanguageText["Tema_Two"].ToString();
                lng_urld.asia = LanguageText["asia"].ToString();
                lng_urld.eu = LanguageText["eu"].ToString();
                lng_urld.com = LanguageText["com"].ToString();
                lng_urld.ru = LanguageText["ru"].ToString();
                lng_urld.CN_S = LanguageText["CN_S"].ToString();
                lng_urld.CN_N = LanguageText["CN_N"].ToString();
                lng_urld.DataGrid_Team_Name = LanguageText["DataGrid_Team_Name"].ToString();
                lng_urld.DataGrid_Team_Win = LanguageText["DataGrid_Team_Win"].ToString();
                lng_urld.DataGrid_Team_Battle = LanguageText["DataGrid_Team_Battle"].ToString();
                lng_urld.DataGrid_Team_power = LanguageText["DataGrid_Team_power"].ToString();
                lng_urld.Select_Server = LanguageText["Select_server"].ToString();
                lng_urld.Testing_Config = LanguageText["Testing_Config"].ToString();
                lng_urld.Net_error = LanguageText["Net_error"].ToString();
                lng_urld.Analysis_data = LanguageText["Analysis_data"].ToString();
                lng_urld.Start_Analysis = LanguageText["Start_Analysis"].ToString();
                lng_urld.Dont_Data_Analysis = LanguageText["Dont_Data_Analysis"].ToString();
                lng_urld.BanList_Head_Text = LanguageText["BanList_Head_Text"].ToString();
                lng_urld.BanTime = LanguageText["BanTime"].ToString();
                lng_urld.BanPlayer_Name = LanguageText["BanPlayer_Name"].ToString();
                lng_urld.Ban_other = LanguageText["Ban_other"].ToString();
                lng_urld.Diy_Ban = LanguageText["Diy_Ban"].ToString();
                lng_urld.Diy_Ban_Time = LanguageText["Diy_Ban_Time"].ToString();
                lng_urld.Diy_Ban_Time_Get = LanguageText["Diy_Ban_Time_Get"].ToString();
                lng_urld.Diy_Ban_Player_Name = LanguageText["Diy_Ban_Player_Name"].ToString();
                lng_urld.Diy_Ban_other = LanguageText["Diy_Ban_other"].ToString();
                lng_urld.Diy_Ban_Add_Button = LanguageText["Diy_Ban_Add_Button"].ToString();
                lng_urld.Diy_Ban_Del_Button = LanguageText["Diy_Ban_Del_Button"].ToString();
                lng_urld.Diy_Ban_ADD_OK = LanguageText["Diy_Ban_ADD_OK"].ToString();
                lng_urld.Diy_Ban_Add_error = LanguageText["Diy_Ban_Add_error"].ToString();
                lng_urld.Diy_Ban_Del = LanguageText["Diy_Ban_Del"].ToString();
                lng_urld.Diy_Ban_Del_error = LanguageText["Diy_Ban_Del_error"].ToString();
                lng_urld.Auto_Del_Ban_Name = LanguageText["Auto_Del_Ban_Name"].ToString();
                lng_urld.updata_log_Tetx = LanguageText["updata_log_Tetx"].ToString();
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
