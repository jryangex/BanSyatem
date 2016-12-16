using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace Core
{
    public class SumData
    {

        public class cn_use_data
        {
            public string D_Name { get; set; } //角色名
            public string D_Win { get; set; } //胜率
            public string D_TR { get; set; } //总场次
            public string D_Rank { get; set; } //信息状态
            public bool D_State { get; set; } //黑名单状态
            //public string D_Hack { get; set; } //留用
            public string D_Ship_cn_Name { get; set; } //战舰中文名

            public string D_Ship_Dcn_Nanme { get; set; }//战舰英文名

            public string D_Ship_ico_Name { get; set; }//当前玩家战舰ico图标名字

            public string D_Ship_cd { get; set; }//当前玩家战舰cd(战舰数字ID)

            public string D_power { get; set; } //效率
        }
        //检测玩家是否在黑名单判断，返回字符串，方便显示
        public static bool Ban_list_cx(string user_name)
        {
            string state;
            if (Core.Sqliteobject.Testing_Name_in_BanList(user_name) == true)
            {
                return true;

            }
                return false;
        }
      
        ////////////////////////////////////////////////////国服专用模块//////////////////////////////////
        public static Dictionary<int, List<cn_use_data>> cn_use_Get_Data_And_Name_Text(Dictionary<int, List<Core.Readerloginfo.Team_Name>> str)
        {
            Dictionary<int, List<cn_use_data>> dad = new Dictionary<int, List<cn_use_data>>();
           
            string url_text_str = "";
            for (int j = 0; j <= str.Count - 1; j++)
            {
                for (int i = 0; i <= str[j].Count - 1; i++)
                {
                    if (url_text_str == "")
                    {
                        url_text_str = str[j][i].Name + ":"+ str[j][i].Shipid;
                    }
                    else
                    {
                        url_text_str = url_text_str + "," + str[j][i].Name + ":" + str[j][i].Shipid;
                    }
                }
            }
            var a = Core.GetUrl.url.API_Server_Url + url_text_str;
            if (GetUserData.Wtr_Get_data_x(a) == false)
            {
                    return null;
            }
            else
            { 
            for (int i=0;i<= str.Count - 1; i++)   //0和1表示队伍分组
                {
                List<cn_use_data> dcd = new List<cn_use_data>();
                    for (int j=0;j<=str[i].Count - 1; j++)  //队伍下的玩家数量
                    {
                      
                        cn_use_data ddcd = new cn_use_data();
                        if (i == 0)
                        {
                            JObject adc = GetUserData.wtr_use_data_p[j];
                            if (str[i][j].Name.Equals(DIC_Bot(str,i,j, adc["name"].ToString()),StringComparison.InvariantCultureIgnoreCase))
                            {
                                ddcd.D_Name = str[i][j].Name.ToString();        //玩家ID
                                ddcd.D_TR = romve_O(adc["battle_num"].ToString());    //玩家总场次
                                ddcd.D_Win = romve_O(adc["win_rate"].ToString());     //玩家胜率
                                ddcd.D_power = romve_O(adc["power"].ToString());
                                ddcd.D_State = Ban_list_cx(DIC_Bot(str, i, j, adc["name"].ToString()));  //是否在黑名检测
                                ddcd.D_Ship_cn_Name = Data_Ship_Name(j, i, str[i][j].Name.ToString());

                                //增加其他
                            x = j;
                            }
                        }
                    else
                        {
                            JObject adbc = GetUserData.wtr_use_data_p[x];               
                            if (str[i][j].Name.Equals(DIC_Bot(str, i, j, adbc["name"].ToString()), StringComparison.InvariantCultureIgnoreCase))
                            {
                                ddcd.D_Name = str[i][j].Name.ToString();
                                ddcd.D_TR = romve_O(adbc["battle_num"].ToString());
                                ddcd.D_Win = romve_O(adbc["win_rate"].ToString());
                                ddcd.D_power = romve_O(adbc["power"].ToString());
                                ddcd.D_State = Ban_list_cx(DIC_Bot(str, i, j, adbc["name"].ToString()));
                                ddcd.D_Ship_cn_Name = Data_Ship_Name(j, i, str[i][j].Name.ToString());
                            //解析数据返回功能增加                             
                             }
                        }
                        x = x+1;
                        dcd.Add(ddcd);
                    }
                dcd.Add(zh_cn_sum_wins(dcd));
                dad.Add(i, dcd);
               
              }
                return dad;
            }
        }
        public static  int x;
        //返回shipname
        static string Data_Ship_Name(int j,int i,string name)
        {
            if (Readerloginfo.All_Team_Data_For_Name_Ship[i][j].Name == name)
            {
                return Readerloginfo.All_Team_Data_For_Name_Ship[i][j].Shipalias;
            }
            else
            {
                return "0";
            }
            
        }
        //计算平均值
        static cn_use_data zh_cn_sum_wins(List<cn_use_data> win)
        {
            double xc =0;
            double oy = 0;
            double oi = 0;
            for (int i=0;i<=win.Count -1; i++)
            {

                double xy = xc + double.Parse(rmove_B(romve_O(win[i].D_Win)));
                xc = xy;
            }
            xc = Math.Round(xc / win.Count, 2);
            for (int i = 0; i <= win.Count - 1; i++)
            {
                double xy = oy + double.Parse(romve_O(win[i].D_TR.ToString()));
                oy = xy;
            }
            oy = Math.Round(oy / win.Count,2);
            for (int i = 0; i <= win.Count - 1; i++)
            {
                double xy = oi + int.Parse(romve_O(win[i].D_power.ToString()));
                oi = xy;
            }
            oi = Math.Round(oi / win.Count,2);
            cn_use_data adf = new cn_use_data();
            adf.D_Name = "团队平均值";
            adf.D_TR = Math.Round(oy,0).ToString();
            adf.D_Win = xc.ToString()+"%";
            adf.D_power = oi.ToString();
            //团队评估预测胜率            
            adf.D_Ship_cn_Name ="预测胜率:"+ Math.Round (( xc * (oi / 100)),0).ToString()+"%";  //预测胜率,保留小数点两位
            return adf;
        }
        static string rmove_B(string str)
        {
            string sx = str.Substring(0, str.Length - 1);
            return sx;
        }
        static string romve_O(string str)
        {
            if (str==null || str == "")
            {
                return "0";
            }
            else
            {
                return str;
            }
        }
        //检测获取的name是否为null or ""返回原数据
        static string DIC_Bot(Dictionary<int, List<Core.Readerloginfo.Team_Name>> _dic,int i,int j,string data_name)
        {
            if (data_name == null || data_name == "")
            {
                return _dic[i][j].Name;
            }else
            {
                return data_name;
            }
        }
    }
}
