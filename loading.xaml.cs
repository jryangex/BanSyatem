using BanSyatem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RAA
{
    /// <summary>
    /// loading.xaml 的交互逻辑
    /// </summary>
    public partial class loading : Window
    {
        public static string m = Directory.GetCurrentDirectory() + "/config.ini";
        public static string n = Directory.GetCurrentDirectory() + "/BanList.sqlite";
        public delegate void loading_Test();
        private Thread threads;
        public loading()
        {

            InitializeComponent();
            threads = new Thread(new ParameterizedThreadStart(Laod_D));
            threads.Start();
           
           
        }
        public MainWindow xnewwin ;
        void Laod_D(object x)
        {
            try
            {

                Thread.Sleep(2000); 
                Core.Detection_Net.Tese_deping();
                Thread.Sleep(1000);
                FSLib.App.SimpleUpdater.Updater.CheckUpdateSimple(Core.Detection_Net.RTSUrl["updata_url"].ToString()); //通过全局url地址检测软件版本更新
                Thread.Sleep(1000);
                if (!Core.Readerloginfo.patch_state(m))
                {
                    FileStream fs = new FileStream(m, FileMode.CreateNew);//创建文件                
                }
                if (!Core.Readerloginfo.patch_state(n))
                {
                    Core.Sqliteobject.Ban_list_Data();
                }
                Thread.Sleep(1000);
                Dispatcher.Invoke(new loading_Test(Show_Main));
            }
          catch
            {
                MessageBox.Show("缺少软件必要运行环境", "软件错误");
                Environment.Exit(0);
                Application.Current.Shutdown();            
            }
        }
        void Show_Main()
        {
            if (xnewwin == null || xnewwin.IsVisible == false)
            {

                xnewwin = new MainWindow();
                xnewwin.Show();
            }
            else
            {
                xnewwin.Activate();
                xnewwin.WindowState = System.Windows.WindowState.Normal;
            }
            Close();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            base.OnMouseLeftButtonDown(e);
            DragMove();
        }


    }
}
