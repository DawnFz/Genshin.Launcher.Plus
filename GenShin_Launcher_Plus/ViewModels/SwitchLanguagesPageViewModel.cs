using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class SwitchLanguagesPageViewModel : ObservableObject
    {
        public SwitchLanguagesPageViewModel()
        {
            SaveLangSetCommand = new RelayCommand(ThisPageRemove);
        }
        public LanguagesModel languages { get => MainBase.lang; }

        public int LangIndex
        {
            get
            {
                return MainBase.IniModel.ReadLang switch
                {
                    "Lang_CN" => 0,
                    "Lang_TW" => 1,
                    "Lang_EN" => 2,
                    _ => -1,
                };
            }
        }
        public List<LanguageListsModel> langlist { get => MainBase.langlist; }

        private string _SwitchLang;
        public string SwitchLang { get => _SwitchLang; set => SetProperty(ref _SwitchLang, value); }
        public ICommand SaveLangSetCommand { get; set; }
        private void ThisPageRemove()
        {
            MainBase.IniModel.ReadLang = SwitchLang;
            MainBase.noab.MainPagesIndex = 0;
            MainWindow mainWindow = new();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
        }
    }
}
