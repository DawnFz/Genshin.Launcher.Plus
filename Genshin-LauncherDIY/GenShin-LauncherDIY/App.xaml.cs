using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows;

namespace GenShin_LauncherDIY
{
    public class SingletonApp
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SingleInstanceManager manager = new SingleInstanceManager();
            manager.Run(args);
        }
    }
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        public void BringWindowTofront()
        {
            //判断是否为托盘状态（Close主窗口不存在）
            if (Current.MainWindow == null)
            {
                //置入托盘的话就新建主窗口并显示
                Current.MainWindow = new MainWindow();
                Current.MainWindow.Show();
            }
            else
            {
                //最小化到任务栏的话就直接还原到前台并激活
                Current.MainWindow.Show();
                Current.MainWindow.WindowState = WindowState.Normal;
                Current.MainWindow.Topmost = true;
                Current.MainWindow.Focus();
                Current.MainWindow.Activate();
            }
        }
    }

    public class SingleInstanceManager : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        private App wpfapp;

        public SingleInstanceManager()
        {
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup(
            Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            wpfapp = new App();
            wpfapp.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            wpfapp.BringWindowTofront();
        }
    }


}
