using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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

        public int LangIndex
        {
            get
            {
                return App.Current.IniModel.ReadLang switch
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
        public ICommand SaveLangSetCommand { get; set; }
        private void ThisPageRemove()
        {
            App.Current.IniModel.ReadLang = SwitchLang;
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
            MainWindow mainWindow = new();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
        }
    }
}
