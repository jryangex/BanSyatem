using RAA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BanSyatem
{
    /// <summary>
    /// Version.xaml 的交互逻辑
    /// </summary>
    public partial class Version : Window
    {
        public Version()
        {
            InitializeComponent();
            try
            {
                textBox_updata_log.Text = Core.ReadUrlText.Get(Core.Detection_Net.RTSUrl["log"].ToString());
                textBox_Sea.Text= Core.ReadUrlText.Get(Core.Detection_Net.RTSUrl["Sea_group"].ToString());
                textBox_Raa.Text= Core.ReadUrlText.Get(Core.Detection_Net.RTSUrl["RAA"].ToString());
            }
            catch
            {
                textBox_updata_log.Text = "网络错误";
            }
            
        }
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }
        //无边框窗体移动
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        //最小化按钮
        private void button_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        public Patch_Set newwinsx;

        private void textBox_Raa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (newwinsx == null || newwinsx.IsVisible == false)
            {

                newwinsx = new Patch_Set();               
                newwinsx.Show();
                Close();
            }
            else
            {
                newwinsx.Activate();
                newwinsx.WindowState = System.Windows.WindowState.Normal;
                Close();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            NativeMethods.SetWindowNoBorder(hwnd);
        }
    }
}
