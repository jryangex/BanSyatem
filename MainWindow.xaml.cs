using RAA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;


namespace BanSyatem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string m = Directory.GetCurrentDirectory() + "/config.ini";
        public static string n = Directory.GetCurrentDirectory() + "/BanList.sqlite";
        public MainWindow()
        {
            InitializeComponent();
                     
            //程序版本号
            label5.Content =System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //载入服务器列表

            //去掉第一列小宽度
            DataGrid_Team_Two.RowHeaderWidth = 0;
            DataGrid_Team_One.RowHeaderWidth = 0;          
            comboBox_lng_list.IsEnabled = false; //关闭语言选择下拉       
            Thread thread_Lng = new Thread(lnglist); //多线程开启获取语言列表和语言数据
            thread_Lng.Start();//执行线程
            //关闭解析按钮,提示选择服务器
            Button_Parsed_Data.IsEnabled = false;
            comboBox_stly.Items.Add("竖版模式");
            comboBox_stly.Items.Add("横版模式");
        }
        /// <summary>
        /// 读取语言线程过程-------------------------------------------------------------------------
        /// </summary>
        ///  //申明线程,声明委托
        public delegate void lng_list_show_c(string str); //语言获取委托
        private void lnglist() 
        {

            if (Core.language.Getlanguagelist() == true)
            {
                for (int i = 0; i <= Core.language.List_h.Count - 1; i++) //通过lng_list.json列出语言列表
                {
                    Dispatcher.Invoke(new lng_list_show_c(combox_lng_list_show), Core.language.List_h[i].language_name);
                }
            }
            else
            {
                MessageBox.Show("获取语言列表错误","语言错误");
            }         
        }
        //增加语言到语言下拉菜单
        private void combox_lng_list_show(string str) 
        {
            comboBox_lng_list.Items.Add(str);
            comboBox_lng_list.IsEnabled = true; //开启语言选择功能
        }
        //风萧萧兮易水寒,壮士一去兮不复还.关闭软件按钮事件
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            Application.Current.Shutdown();

        }
        /// wpf鼠标左键移动窗体事件
        protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e)
        {

            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        //软件窗体最小化按钮
        private void button_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        public static List<string> url;
        //玩家选择服务器后,对解析环境进行检测
        public delegate void Test_config_De(bool str);//选择服务器后对解析环境检测线程委托
        private void comboBox_state_Server_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                Thread Test_config = new Thread(new ParameterizedThreadStart(Test_config_Test)); 
                Test_config.Start(comboBox_state_Server_list.SelectedIndex); //启动线程,并且通过object传入玩家选择服务器
                Button_Parsed_Data.IsEnabled = false; //关闭服务器解析按钮,防止误操作
                comboBox_state_Server_list.IsEnabled = false;//关闭服务器下拉按钮,防止误操作
                Button_Parsed_Data.Content = Core.language .lng_urld .Testing_Config;   //解析按钮文本显示环境检测,,,,后期需要调用全局语言包进行多国语言增加                      
        }
        /// 环境检测过程
        public void Test_config_Test(object x) 
        {         
            int aac = int.Parse(x.ToString());//把传入玩家控件x丢转换成int并且赋值给aac
            Core.GetUrl.Get_server_url(aac);//拟合查询api
            //结束判定条件获取委托给下面地址检测操作
            Dispatcher.Invoke(new Test_config_De(Test_over_Test), Core.Readerloginfo.patch_state(Core.IniHelper.z_patch(m) + "\\profile\\python.log")); 
        }
        public void Test_over_Test(bool stc)
        {
            if (stc == false)  //如果检测log返回为false
            {
                Lable_data.Content = "解析客户端路径错误!";
                Button_Parsed_Data.IsEnabled = false;
                comboBox_state_Server_list.IsEnabled = true;
                Button_Parsed_Data.Content = Core.language.lng_urld.Net_error;
                if (pnewwin == null || pnewwin.IsVisible == false)
                {

                    pnewwin = new Patch_Set();
                    pnewwin.Show();
                }
                else
                {
                    pnewwin.Activate();
                    pnewwin.WindowState = System.Windows.WindowState.Normal;
                }
            }
            else
            {
                Button_Parsed_Data.IsEnabled = true;
                comboBox_state_Server_list.IsEnabled = true;
                Button_Parsed_Data.Content = Core.language.lng_urld.Parsed_Data;
               
            }
        }
        /// ////////////////////////////开始解析//////////////////////////////////////////////
        Dictionary<string, List<string>> dib = new Dictionary<string, List<string>>();
        private void button_Click(object sender, RoutedEventArgs e)
        {
            threads = new Thread(new ParameterizedThreadStart(zh_cn_Laod_Data));
            threads.Start();
            Button_Parsed_Data.IsEnabled = false;
            Button_Parsed_Data.Content = Core.language .lng_urld .Analysis_data ;
            comboBox_state_Server_list.IsEnabled = false;

        }
        private Thread threads;
        //多线程加载玩家的信息,单批12个线程,两个队伍24线程
        // 创建委托
        public delegate void Load_Use_Data(Dictionary<int, List<Core.SumData.cn_use_data>> str);
        public delegate void server_error();
        public delegate void Show_pross(double str);
        //线程过程
        public void zh_cn_Laod_Data(object xx)
        {
            if (Core.Readerloginfo.ret_txt(Core.IniHelper.z_patch(m)+ "\\profile\\python.log") == true)
            {
                Dictionary<int, List<Core.SumData.cn_use_data>> ads = Core.SumData.cn_use_Get_Data_And_Name_Text(Core.Readerloginfo.All_Team_Data_For_Name_Ship);
                if (ads == null)
                {
                    Dispatcher.Invoke(new server_error(error));
                }
                else
                {
                    Dispatcher.Invoke(new Load_Use_Data(Show_Data), ads);
                }               
            }
            else
            {
                Dispatcher.Invoke(new server_error(error));
            }          
        }
        //控件显示数据
        public void Show_Data(Dictionary<int, List<Core.SumData.cn_use_data>> Show_Data)
        {
            DataGrid_Team_One.DataContext = Show_Data[0];         
            DataGrid_Team_Two.DataContext = Show_Data[1];
            Button_Parsed_Data.Content = Core.language.lng_urld .Start_Analysis;
            Button_Parsed_Data.IsEnabled = true;
            comboBox_state_Server_list.IsEnabled = true;
            Lable_data.Content =null;  //解析正确完全显示后,对状态或者出错显示控件进行清空
        }
        //计算队伍平均值
        //当前控件内的玩家如果在黑名单内,高亮显示该玩家行
        private void dataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            
            DataGridRow dataGridRow = e.Row;
            var adffc = (Core.SumData.cn_use_data)dataGridRow.DataContext;
            if (adffc.D_State == true)
            {
                dataGridRow.Background = System.Windows.Media.Brushes.Plum;
            }
        }

        public void error()
        {
            Lable_data.Content = Core.language .lng_urld .Dont_Data_Analysis;
            Button_Parsed_Data.IsEnabled = true;
            comboBox_state_Server_list.IsEnabled = true;         
        }
        public Patch_Set pnewwin;
        public BlackList newwin;
        public Version newwins;

        //鼠标右键选中行事件上面表
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var adc = (Core.SumData.cn_use_data)DataGrid_Team_One.SelectedItem;
            try
            {
                Core.Sqliteobject.input_data(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), adc.D_Name.ToString(), "无条件拉黑");
                newwin.Show_Ban_List_And_Updata_object();
            }
            catch
            {
                MessageBox.Show("没有选中任何可增加数据", "软件提示");
            }

        }
        //下面表
        private void MenuItem_Click_two(object sender, RoutedEventArgs e)
        {

            var adc = (Core.SumData.cn_use_data)DataGrid_Team_Two.SelectedItem;
            try
            {
                Core.Sqliteobject.input_data(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), adc.D_Name.ToString(), "无条件拉黑");
            }
            catch
            {
                MessageBox.Show("没有选中任何可增加数据", "软件提示");
            }

        }
        //版本授权信息
        private void label5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (newwins == null || newwins.IsVisible == false)
            {

                newwins = new Version();
                newwins.Show();
            }
            else
            {
                newwins.Activate();
                newwins.WindowState = System.Windows.WindowState.Normal;
            }
        }
        //语言线程
        public delegate void lng_switch_lng_show(Core.language.language_data str);
        private Thread switch_lng;
        private void comboBox_lng(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_lng_list.SelectedIndex == -1)
            {
                MessageBox.Show("获取列表","语言出错");
            }else
            {
                switch_lng = new Thread(switchlng);
                switch_lng.Start(comboBox_lng_list.SelectedIndex);
            }        
        }
        private void switchlng(object xix)
        {
            if(Core.language.GetLanguagedata(Core.language.List_h, int.Parse(xix.ToString()))==true)
            {
                Dispatcher.Invoke(new lng_switch_lng_show(show_lng_text), Core.language.lng_urld);
            }else
            {
                MessageBox.Show("获取语言文本出错","语言出错");
            }
           
        }
        private void show_lng_text(Core.language.language_data adf)
        {
            label_BanSystem.Content = adf.BanSystem;
            Label_Game_Server.Content = adf.Game_Server;
            Button_Parsed_Data.Content = adf.Parsed_Data;
            Label_Team_One.Content = adf.Team_One;
            Label_Team_Two.Content = adf.Tema_Two;
            comboBox_state_Server_list.Items.Clear();
            DataGrid_Team_One.Columns[0].Header = adf.DataGrid_Team_Name;
            DataGrid_Team_One.Columns[2].Header = adf.DataGrid_Team_Win;
            DataGrid_Team_One.Columns[3].Header = adf.DataGrid_Team_Battle;
            DataGrid_Team_One.Columns[4].Header = adf.DataGrid_Team_power;
            //------------------------------------------------------------------
            DataGrid_Team_Two.Columns[0].Header = adf.DataGrid_Team_Name;
            DataGrid_Team_Two.Columns[2].Header = adf.DataGrid_Team_Win;
            DataGrid_Team_Two.Columns[3].Header = adf.DataGrid_Team_Battle;
            DataGrid_Team_Two.Columns[4].Header = adf.DataGrid_Team_power;
            for (int i = 0; i <= 5; i++)
            {
                comboBox_state_Server_list.Items.Add(Core.GetUrl.Function_Server_list_name(i));
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("本功能暂时未开放","软件提示");
        }
        //黑名单窗体
        private void label_BanSystem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (newwin == null || newwin.IsVisible == false)
            {

                newwin = new BlackList();
                newwin.Show();
            }
            else
            {
                newwin.Activate();
                newwin.WindowState = System.Windows.WindowState.Normal;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            NativeMethods.SetWindowNoBorder(hwnd);
        }
        private void comboBox_stly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_stly.SelectedIndex == 0)
            {
                this.Width = 398;
                this.Height = 761;
                Thickness margin = new Thickness(0, 470, 43, 0);
                Thickness marginx = new Thickness(0, 429, 178, 0);
                DataGrid_Team_Two.Margin = margin;
                Label_Team_Two.Margin = marginx;
                label_VS.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Width = 900;
                this.Height = 500;
                Thickness margin = new Thickness(0, 164, 43, 0);
                Thickness marginx = new Thickness(0, 124, 177, 0);
                Thickness marginvs = new Thickness(0, 263, 410, 0);
                DataGrid_Team_Two.Margin = margin;
                Label_Team_Two.Margin = marginx;
                label_VS.Margin = marginvs;
                label_VS.Visibility = Visibility.Visible;
            }
        }
    }
    
}
