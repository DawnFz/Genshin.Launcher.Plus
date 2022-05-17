using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace GenShin_Launcher_Plus
{
    public partial class App : Application
    {
        private readonly SingleInstanceChecker singleInstanceChecker = new("GenshinLauncherPlus");
        public App()
        {
            LoadProgramCore = new();
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            RunPathCheck();
            //单例检查
            singleInstanceChecker.Ensure(this, BringWindowToFront<MainWindow>);
            base.OnStartup(e);
        }

        /// <summary>
        /// 用于检查程序是否运行在游戏目录
        /// </summary>
        private void RunPathCheck()
        {
            if (File.Exists(@"YuanShen.exe") &&
                Directory.Exists(@"YuanShen_Data") ||
                File.Exists(@"GenshinImpact.exe") &&
                Directory.Exists(@"GenshinImpact_Data"))
            {
                MessageBox.Show("Error: This program cannot be running in this path!");

                if (Directory.Exists(@"UserData"))
                {
                    Directory.Delete(@"UserData", true);
                }
                if (Directory.Exists(@"Config"))
                {
                    Directory.Delete(@"Config", true);
                }
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 查找 <see cref="App.Current.Windows"/> 集合中的对应 <typeparamref name="TWindow"/> 类型的 Window
        /// </summary>
        public static void BringWindowToFront<TWindow>() where TWindow : Window, new()
        {
            TWindow? window = Current.Windows.OfType<TWindow>().FirstOrDefault();

            if (window is null)
            {
                window = new();
            }
            if (window.WindowState == WindowState.Minimized || window.Visibility != Visibility.Visible)
            {
                window.Show();
                window.WindowState = WindowState.Normal;
            }
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
            window.Focus();
        }

        public new static App Current => (App)Application.Current;

        public LoadProgramCore LoadProgramCore { get; set; }

        public DataModel DataModel { get; set; }

        public List<LanguageListModel> LangList { get; set; }

        public NoticeOverAllBase NoticeOverAllBase { get; set; }

        public LanguageModel Language { get; set; }

        public UpdateModel? UpdateObject { get; set; }

        public MainWindow ThisMainWindow { get; set; }

        public bool IsLoading { get; set; }

    }
}
