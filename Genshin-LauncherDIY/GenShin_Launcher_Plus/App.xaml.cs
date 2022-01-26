using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows;

namespace GenShin_Launcher_Plus
{
    public class SingletonApp
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SingleInstanceManager manager = new();
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
            Current.Dispatcher.Invoke(() =>
            {
                Window window = Window.GetWindow(MainWindow);
                //判断是否为托盘状态（Close主窗口不存在）
                if (window == null)
                {
                    //置入托盘的话就新建主窗口并显示
                    Window mainwindow = new MainWindow();
                    mainwindow.Show();
                }
                else
                {
                    //最小化到任务栏的话就直接还原到前台并激活
                    window.Show();
                    window.WindowState = WindowState.Normal;
                    window.Topmost = true;
                    window.Focus();
                    window.Activate();
                }
            });
        }
    }

    public class SingleInstanceManager : WindowsFormsApplicationBase
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
