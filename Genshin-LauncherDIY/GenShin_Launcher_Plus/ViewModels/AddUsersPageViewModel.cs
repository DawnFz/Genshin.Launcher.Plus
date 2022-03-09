using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class AddUsersPageViewModel : ObservableObject
    {

        public AddUsersPageViewModel()
        {
            languages = MainBase.lang;
            CreateGamePortList();
            SaveUserDataCommand = new RelayCommand(SaveUserData);
        }

        public LanguagesModel languages { get; set; }

        public string GamePort { get; set; }
        public string Name { get; set; }

        //游戏客户端列表
        private List<GamePortListModel> _GamePortLists;
        public List<GamePortListModel> GamePortLists
        {
            get => _GamePortLists;
            set => SetProperty(ref _GamePortLists, value);
        }
        private void CreateGamePortList()
        {
            GamePortLists = new();
            GamePortLists.Add(new GamePortListModel { GamePort = languages.GameClientTypePStr });
            GamePortLists.Add(new GamePortListModel { GamePort = languages.GameClientTypeMStr });
        }

        public ICommand SaveUserDataCommand { get; set; }
        private void SaveUserData()
        {
            if (GamePort == languages.GameClientTypePStr && Name != null)
            {
                RegistryControl registryControl = new();
                string userdata = registryControl.GetFromRegedit(Name, "CN");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else if (GamePort == languages.GameClientTypeMStr && Name != null)
            {
                RegistryControl registryControl = new();
                string userdata = registryControl.GetFromRegedit(Name, "Global");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else
            {
                MessageBox.Show(languages.AddUsersErrorMessageStr);
            }
        }
    }
}
