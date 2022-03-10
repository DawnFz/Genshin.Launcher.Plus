using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class SwitchLanguagesPageViewModel : ObservableObject
    {
        public SwitchLanguagesPageViewModel()
        {
            langlist = MainBase.langlist;
            languages = MainBase.lang;
            SaveLangSetCommand = new RelayCommand(ThisPageRemove);
        }
        public LanguagesModel languages { get; set; }

        private List<LanguageListsModel> _langlist;
        public List<LanguageListsModel> langlist { get => _langlist; set => _langlist = value; }

        private string _SwitchLang;
        public string SwitchLang { get => _SwitchLang; set => SetProperty(ref _SwitchLang,value); }
        public ICommand SaveLangSetCommand { get; set; }
        private void ThisPageRemove()
        {
            IniControl.ReadLang = SwitchLang;
            LoadLangCore();
            MainWindow mainWindow = new();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
        }
        public void LoadLangCore()
        {
            LoadProgramCore.LoadLanguageCore(SwitchLang);
        }
    }
}
