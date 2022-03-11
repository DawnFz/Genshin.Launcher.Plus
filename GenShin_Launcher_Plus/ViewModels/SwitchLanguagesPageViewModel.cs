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
            languages = MainBase.lang;
            SaveLangSetCommand = new RelayCommand(ThisPageRemove);
        }
        public LanguagesModel languages { get; set; }

        public List<LanguageListsModel> langlist{ get => MainBase.langlist; }

        private string _SwitchLang;
        public string SwitchLang { get => _SwitchLang; set => SetProperty(ref _SwitchLang,value); }
        public ICommand SaveLangSetCommand { get; set; }
        private void ThisPageRemove()
        {
            MainBase.IniModel.ReadLang = SwitchLang;
            MainWindow mainWindow = new();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainWindow;
        }
    }
}
