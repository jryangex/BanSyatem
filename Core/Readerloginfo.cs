using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core
{
    public class Readerloginfo
    {
        //检测日志是否存在
        public static bool patch_state(string patch)
        {
            return File.Exists(patch);
        }
        //解析日志
        public static bool ret_txt(string patch)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<string> list_Get = Read(patch); //读取txt文件内容并赋给list_Get内  
            foreach (string s in list_Get)
            {
                int arr = s.IndexOf("player: Id"); //遍历出含有指定字符串并丢给数组
                if (arr != -1)
                {
                    string sl = s.ToString().Substring(5, 19);  //名称
                    string sx = s.ToString().Substring(25, s.ToString().Length - 25); //值                                        
                    if (dic.ContainsKey(sl)) //判断表中是否存在相同名称,不存在则重新建立,存在就直接插入数据
                    {   //直接插入数据
                        List<string> listkeys = new List<string>();
                        listkeys.Add(sx);
                        dic[sl].InsertRange(dic[sl].Count, listkeys);
                      
                    }
                    else
                    {   //新建一个日期表,并且插入数据
                        List<string> listKeys = new List<string>();
                        listKeys.Add(sx);
                        dic.Add(sl, listKeys);                
                    }

                }

            }
            List<string> Team_And_Me = new List<string>();
            if (dic.Count >0)  //判断是否有战斗数据，提取最后一次战斗玩家24人所有
            {
               for(int i = 0; i <= list_Get.Count - 1; i++)
                {
                   if(list_Get[i].IndexOf(dic[dic.Keys.Last<string>()][0].ToString()) != -1)
                    {
                        Team_And_Me.Add(list_Get[i - 5]); //返回team id行
                        Team_And_Me.Add(list_Get[i - 6]); //返回本机客户端玩家Name行
                    }
                }
                Player_Team_And(dic[dic.Keys.Last<string>()], Team_And_Me); 
                return true;
            }
            else
            {
                return false;
            }           
        }
        //读取日志
         static  List<string> Read(string path)
        {
            //处理一个BUG,解决一个文件多个进程读取问题
            FileStream filse_log = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(filse_log, Encoding.UTF8);
            string line;
            List<string> list = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line.ToString());
            }
            return list;
        }
       public static Dictionary<int, List<Team_Name>> All_Team_Data_For_Name_Ship = new Dictionary<int, List<Team_Name>>();
        //list name
        public static void Player_Team_And(List<string> data,List<string> data_u)
        {
            All_Team_Data_For_Name_Ship.Clear(); 
            Player_data_Z(data_u);//获取主玩家Name和Team ID
            var acd= player_name_team(data);
            List<Team_Name> _afgh = new List<Team_Name>();
            List<Team_Name> _afgf = new List<Team_Name>();
            for (int i = 0; i <= acd.Count - 1; i++) //循环分配.
            {                
                if(acd[i].TeamID ==_z_user_data._zTeamid) 
                {
                    _afgh.Add(acd[i]);               
                }
                else
                {
                    _afgf.Add(acd[i]);                  
                }
            }
            All_Team_Data_For_Name_Ship.Add(0, _afgh);
            All_Team_Data_For_Name_Ship.Add(1, _afgf);
        }
        //返回主Name的数据
        public class use_data
        {
            public string _zname { get; set; }
            public int _zTeamid { get; set; }
        }
        static use_data _z_user_data = new use_data(); //全局储存本地客户端玩家ID，TeamID数据
        private static void Player_data_Z(List<string> data_u)  //获取本地客户端玩家ID，TeamID
        {
            for(int i=0;i<= data_u.Count - 1; i++)
            {
                if(data_u[i].IndexOf("name ")!= -1)
                {
                    var c_use_name = data_u[i].IndexOf("name ");
                    _z_user_data._zname = data_u[i].Substring(c_use_name+5, data_u[i].Length -c_use_name -5);
                }
              
            }
            for (int i = 0; i <= data_u.Count - 1; i++)
            {
                if(data_u[i].IndexOf("team id ")!=-1)
                {
                    var c_use_name = data_u[i].IndexOf("team id ");
                    _z_user_data._zTeamid = int.Parse(data_u[i].Substring(c_use_name + 8, data_u[i].Length - c_use_name - 8));
                }              
            }
        }
        public class Team_Name //超级变态表
        {
            public string Name { get; set; } //玩家名字             查询
            public int TeamID { get; set; } //玩家团队ID            用于区分友军敌军
            public string Shipid { get; set; } //玩家当前战舰id     调取对应图标
            public long Shipcd { get; set; }//玩家战舰CD             查询对应战舰数据
            public string Shipalias { get; set; }//玩家战舰中文名   显示战舰名字中文语言显示所用
            public string Shipname { get; set; }//玩家战舰英文名    非中文语言显示所用

        }

        public class Ship_Data_z
        {
            public long Shipcd { get; set; }//玩家战舰CD             查询对应战舰数据
            public string Shipalias { get; set; }//玩家战舰中文名   显示战舰名字中文语言显示所用
            public string Shipname { get; set; }//玩家战舰英文名    非中文语言显示所用
        }
         static string _Ship_data;
        //通过玩家战舰id获取玩家战舰cd和战舰中文名，战舰英文名。
        public static List<Team_Name> player_name_team(List<string> All_use_team_data) 
        {
            List<Team_Name> dab = new List<Team_Name>();
            if (GetUserData.Get_data_x("http://rank.kongzhong.com/wows/scripts/ships.js") == true)
            {
                _Ship_data = ship_data_edit(GetUserData .ship_data_p);
               
                //申明数组                         
                for (int i = 0; i <= All_use_team_data.Count - 1; i++)
                {
                    Team_Name ddc = new Team_Name();
                    ddc.Name = GunctionPlayer_Name(All_use_team_data, i);
                    ddc.TeamID = GunctionPlayer_Team_ID(All_use_team_data, i);
                    ddc.Shipid = GunctionPlayer_Ship_Name(All_use_team_data, i);
                    ddc.Shipcd = GunctionShipid(_Ship_data, ddc.Shipid).Shipcd;
                    ddc.Shipalias = dsz.Shipalias;
                    ddc.Shipname = dsz.Shipname;
                    dab.Add(ddc);
                }
                return dab;   //名字，团队ID，战舰名字，战舰ID，战舰中文，战舰英文
            }else
            {
                return null;
            }
          
        }
        //处理远程战舰列表数据
        static string ship_data_edit(string ship_data)
        {
            var d_d=ship_data.IndexOf("var shipDict=");
            return ship_data.Substring(d_d+13, ship_data.Length-d_d-13);
        }
        public static Ship_Data_z dsz = new Ship_Data_z();
        static Ship_Data_z GunctionShipid(string str, string shipid)
        {
            var ytu=GetJsonValue(str);
            for (int i = 0; i <= ytu.Count - 1; i++)
            {

                if (ytu[i]["id"].ToString() == shipid)
                {

                    dsz.Shipname = ytu[i]["name"].ToString();
                    dsz.Shipcd = long.Parse((ytu[i]["cd"].ToString()));
                    dsz.Shipalias = ytu[i]["alias"].ToString();
                }
            }
            return dsz;   //返回战舰名字，CD,alias
        }

         static List<JObject> GetJsonValue(string strJson)
        {
            string strResult;
            JObject jo = JObject.Parse(strJson);
            List<JObject> dsz = new List<JObject>();
            string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
            if (values == null)
            {
                strResult = "";
            }
            else
            {
                for(int i = 0; i <= values.LongLength - 1; i++)
                {

                    dsz.Add((JObject)JsonConvert.DeserializeObject(values[i]));
                }
                
            }
            return dsz;  //直接提取json的value加入到list<Jobject>并且返回
        }
        //返回玩家ID
        static string GunctionPlayer_Name(List<string> str,int ix)
        {
            string firstSTR = "Name: ";
            string secondSite = " TeamId";
            var zname = str[ix].IndexOf(firstSTR, 0);
            var sname = str[ix].IndexOf(secondSite, zname + 1);           
            return str[ix].Substring(zname + firstSTR.Length, sname - zname - firstSTR.Length);
        }
        //返回玩家战舰名
        static string GunctionPlayer_Ship_Name(List<string> str, int ix)
        {
            string ads = "ShipName: ";
            int sc = str[ix].IndexOf(ads, 0);
            return str[ix].Substring(sc + ads.Length, 7);
        }
        //返回玩家 团队ID
        static int GunctionPlayer_Team_ID(List<string> str, int ix)
        {
            string addc = "TeamId: ";
            int scc = str[ix].IndexOf(addc, 0);
            return int.Parse(str[ix].Substring(scc + addc.Length, 1));
        }

    }
}
