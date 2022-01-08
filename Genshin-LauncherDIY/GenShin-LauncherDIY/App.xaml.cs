using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace GenShin_LauncherDIY
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;

        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "ElectronicNeedleTherapySystem", out ret);
            if (!ret)
            {
                MessageBox.Show("你正在运行另一个原神启动器Plus，禁止重复打开！！");
                Environment.Exit(0);
            }
        }
    }
}
