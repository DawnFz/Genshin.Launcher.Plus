using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.ViewModels;
using System.Collections.Generic;
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
            singleInstanceChecker.Ensure(this, BringWindowToFront<MainWindow>);
            base.OnStartup(e);
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

        public IniModel IniModel { get; set; }

        public List<LanguageListModel> LangList { get; set; }

        public NoticeOverAllBase NoticeOverAllBase { get; set; }

        public LanguageModel Language { get; set; }

        public UpdateModel? UpdateObject { get; set; }

        public bool IsLoading { get; set; }

    }
}
