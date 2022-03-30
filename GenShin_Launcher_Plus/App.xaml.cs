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
            _IniModel = new();
            _NoticeOverAllBase = new();
            new LoadProgramCore().LoadUpdateCore();
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

        private IniModel _IniModel;
        public IniModel IniModel { get => _IniModel; set => _IniModel = value; }

        private List<LanguageListModel> _LangList;
        public List<LanguageListModel> LangList { get => _LangList; set => _LangList = value; }

        private NoticeOverAllBase _NoticeOverAllBase;
        public NoticeOverAllBase NoticeOverAllBase { get => _NoticeOverAllBase; }

        private LanguageModel _Language;
        public LanguageModel Language { get => _Language; set => _Language = value; }

        private UpdateModel _UpdateObject;
        public UpdateModel UpdateObject { get => _UpdateObject; set => _UpdateObject = value; }

        private SettingsPageViewModel _SettingsPageViewModel;
        public SettingsPageViewModel SettingsPageViewModel { get => _SettingsPageViewModel; set => _SettingsPageViewModel = value; }

    }
}
