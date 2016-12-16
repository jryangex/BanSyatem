using System.Data.SQLite;
using System.IO;

namespace Core
{
    public class Sqliteobject
    {
        //---------------------------------JackLee-2016-11-15-----------------------//
        //创建数据库和数据库表
        public static string BanList_Data_PassWord = "00d115c27c5d243e3f0b22547961a575edd1cd8195d7c9d5cd4f609d8d7c029b194331de87b6ce4d98580d1160c2ff693257518e5dd46e619545233b70e68d67";
        public static void Ban_list_Data()
        {
            if (!File.Exists(System.Environment.CurrentDirectory + "\\BanList.sqlite"))
            {
                SQLiteConnection cnn = new SQLiteConnection();
                SQLiteConnection.CreateFile("BanList.sqlite");
                cnn = new SQLiteConnection("Data Source=BanList.sqlite;Version=3;");
                cnn.Open();
                cnn.ChangePassword(BanList_Data_PassWord);
                string sql = "create table BanList_List (time varchar(20), Name varchar(60),Remarks varchar(80))";
                SQLiteCommand command = new SQLiteCommand(sql, cnn);
                command.ExecuteNonQuery();
                cnn.Close();
            }        
        }
        //连接数据库
        public static  SQLiteConnection connect_list_Data()
        {
            SQLiteConnection cnn = new SQLiteConnection();
            try
            {
                cnn = new SQLiteConnection("Data Source=BanList.sqlite;Version=3;");
                cnn.SetPassword(BanList_Data_PassWord);
                cnn.Open();
                return (cnn);   //连接数据库,正常返回连接
            }
            catch
            {
                return null; //异常返回空
            }
                    
        }
        public static void input_data(string time, string name, string Remarks)
        {
            chn = connect_list_Data();
            //插入数据
            string sql = "insert into BanList_List (time, Name,Remarks) values ('" + time + "', '" + name + "','" + Remarks + "')";
            if (Testing_Name_in_BanList(name) == false)
            {
                SQLiteCommand commands = new SQLiteCommand(sql, chn);
                commands.ExecuteNonQuery();
                System.Windows.MessageBox.Show("数据插入成功", "系统提示");            
                chn.Close();

            }
            else
            {
                System.Windows.MessageBox.Show(name + "数据已经存在", "系统提示");
                chn.Close();
            }
        }

        //删除数据
        public static void Del_data(string name)
        {
            chn = connect_list_Data();
            string sql = "delete from BanList_List where Name = '" + name + "'";
            if (Testing_Name_in_BanList(name) == true)
            {
                SQLiteCommand commands = new SQLiteCommand(sql, chn);
                commands.ExecuteNonQuery();
                System.Windows.MessageBox.Show("数据删除成功", "系统提示");
                chn.Close();
            }
            else
            {
                System.Windows.MessageBox.Show(name + "数据不存在", "系统提示");
                chn.Close();
            }
           
        }
        public static SQLiteConnection chn;
        //检测Name是否在BanList列表
        public static bool Testing_Name_in_BanList(string name)
        {
            chn = connect_list_Data();
            string sql_name = "select * from BanList_List  where Name like '" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql_name, chn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    reader.Close();
                    return true;

                }
                else
                {
                    reader.Close();
                    return false;

                }        
               
            
        }
    }
}
