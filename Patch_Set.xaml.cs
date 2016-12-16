
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Interop;

namespace RAA
{

    /// <summary>
    /// Patch_Set.xaml 的交互逻辑
    /// </summary>
    public partial class Patch_Set : Window
    {
        public Patch_Set()
        {
            InitializeComponent();
                if (!File.Exists(patchc))
                {
                    FileStream fs = new FileStream(patchc, FileMode.CreateNew);//创建文件                
                }
                else
                {
                    Core.IniHelper ini = new  Core.IniHelper(patchc);
                    textBox_Patch.Text = ini.ReadValue("Clent", "Patch");
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
        string patchc = Directory.GetCurrentDirectory() + "/config.ini";
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string tmp_path = "";
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
            {
                tmp_path = fbd.SelectedPath;
            }
            
            // 写入ini       
            Core.IniHelper ini = new Core.IniHelper(patchc);
            ini.WriteValue("Clent", "Patch", tmp_path);
            //读取
            textBox_Patch.Text = ini.ReadValue("Clent", "Patch");
            patchc = tmp_path;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            NativeMethods.SetWindowNoBorder(hwnd);
        }
    }
}
