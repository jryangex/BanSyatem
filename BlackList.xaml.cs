using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace BanSyatem
{
    /// <summary>
    /// BlackList.xaml 的交互逻辑
    /// </summary>
    public partial class BlackList : Window
    {
        public delegate  void Show_Updata_Ban_List(SQLiteDataReader str);
        private Thread Show_Ban_List_And_Updata;
        public BlackList()
        {
           InitializeComponent();
           DataGrid_Ban_List.RowHeaderWidth = 0; //设置第一列小宽度
           label_Heard_Text.Content =Core.language .lng_urld.BanList_Head_Text;
            DataGrid_Ban_List.Columns[0].Header = Core.language.lng_urld.BanTime;
            DataGrid_Ban_List.Columns[2].Header = Core.language.lng_urld.BanPlayer_Name; 
            DataGrid_Ban_List.Columns[3].Header = Core.language.lng_urld.Ban_other;
            TabItem_Text.Header  = Core.language.lng_urld.Diy_Ban;
            Label_Time.Content = Core.language.lng_urld.Diy_Ban_Time;
            Button_Get_time.Content = Core.language.lng_urld.Diy_Ban_Time_Get;
            Lable_Name.Content = Core.language.lng_urld.Diy_Ban_Player_Name;
            Label_Other.Content = Core.language.lng_urld.Diy_Ban_other;
            Button_Add.Content = Core.language.lng_urld.Diy_Ban_Add_Button;
            Button_Del.Content = Core.language.lng_urld.Diy_Ban_Del_Button;
            if (!Core.Readerloginfo.patch_state(MainWindow.n))
            {
               Core.Sqliteobject.Ban_list_Data();
            }
            else
            {
                Show_Ban_List_And_Updata = new Thread(new ThreadStart(Show_Ban_List_And_Updata_object));
                Show_Ban_List_And_Updata.Start();
            }
           
        }
        public void Show_Ban_List_And_Updata_object()
        {           
            string sql = "select time,name,Remarks from BanList_List";
            SQLiteCommand command = new SQLiteCommand(sql, Core.Sqliteobject .connect_list_Data());
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();
                Dispatcher.Invoke(new Show_Updata_Ban_List(Show_conl_Ban_Updata_List), reader);
            }
            catch
            {
            }
        }
        public class Member
        {
            public string DTime{ get; set; }
            public string DName { get; set; }
            public string DBZ { get; set; }

        }
        
        public void Show_conl_Ban_Updata_List(SQLiteDataReader str)
        {
            ObservableCollection<Member> memberData = new ObservableCollection<Member>();
            while (str.Read())
                //MessageBox.Show(str["time"].ToString());
            memberData.Add(new Member(){DTime = str["time"].ToString(),DName = str["name"].ToString(),DBZ = str["Remarks"].ToString()});
            DataGrid_Ban_List.DataContext = memberData;
            //listBox_info.Items.Add(str["time"].ToString() + "          " + str["name"].ToString() + "                    " + str["Remarks"].ToString());
        }
        //无边框窗体移动
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
           
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //增加数据
        private void button1_Click(object sender, RoutedEventArgs e)
        {
           if ((TextBox_Time.Text!="")&&(TextBox_Name.Text != ""))
            {
                Core.Sqliteobject .input_data(TextBox_Time.Text, TextBox_Name.Text, TextBox_Remarks.Text);
                Show_Ban_List_And_Updata = new Thread(new ThreadStart(Show_Ban_List_And_Updata_object));
                Show_Ban_List_And_Updata.Start();

            }
            else
            {
                MessageBox.Show("请完善手动增加信息", "错误提示");
            }
        }
        //获取时间
        private void button_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Time .Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        //删除数据
        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            if((TextBox_Name.Text != ""))
            {
                Core.Sqliteobject .Del_data(TextBox_Name.Text);
                Show_Ban_List_And_Updata = new Thread(new ThreadStart(Show_Ban_List_And_Updata_object));
                Show_Ban_List_And_Updata.Start();

            }
            else
            {
                MessageBox .Show("请输入要删除的ID在ID栏!", "错误提示");
            }

        }
        //右键删除操作
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var addf = (Member)DataGrid_Ban_List.SelectedItem;
            if (addf==null)
            {
                MessageBox.Show("无数据可删", "错误提示");
            }else
            {
                Core.Sqliteobject.Del_data(addf.DName);
            }             
            //子线程刷新列表
            Show_Ban_List_And_Updata = new Thread(new ThreadStart(Show_Ban_List_And_Updata_object));
            Show_Ban_List_And_Updata.Start();
        }

    }

}
