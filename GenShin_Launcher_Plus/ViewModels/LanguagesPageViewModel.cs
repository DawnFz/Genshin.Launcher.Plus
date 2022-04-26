using GenShin_Launcher_Plus.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class LanguagesPageViewModel : ObservableObject
    {
        public LanguagesPageViewModel()
        {
            SaveLangSetCommand = new RelayCommand(ThisPageRemove);
        }
        public LanguageModel languages { get => App.Current.Language; }

        /// <summary>
        /// 从设置文件中获得选中的语言
        /// </summary>
        public int LangIndex
        {
            get
            {
                return App.Current.DataModel.ReadLang switch
                {
                    "Lang_CN" => 0,
                    "Lang_TW" => 1,
                    "Lang_EN" => 2,
                    _ => -1,
                };
            }
        }
        public List<LanguageListModel> langlist { get => App.Current.LangList; }

        private string _SwitchLang;
        public string SwitchLang { get => _SwitchLang; set => SetProperty(ref _SwitchLang, value); }

        /// <summary>
        /// 保存语言后重新创建窗口实现更新语言
        /// </summary>
        public ICommand SaveLangSetCommand { get; set; }
        private void ThisPageRemove()
        {
            App.Current.DataModel.ReadLang = SwitchLang;
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
            MainWindow mainWindow = new();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
        }
    }
}
