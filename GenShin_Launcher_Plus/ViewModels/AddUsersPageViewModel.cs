using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;



namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 这个类是AddUsersPage的ViewModel 
    /// 集成了AddUsersPage的部分操作实现逻辑
    /// </summary>
    public class AddUsersPageViewModel : ObservableObject
    {

        public AddUsersPageViewModel()
        {
            CreateGamePortList();
            SaveUserDataCommand = new RelayCommand(SaveUserData);
        }

        public LanguagesModel languages { get => MainBase.lang; }

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
                try
                {
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
                }
                catch
                {
                    MessageBox.Show($"Error : {Name}");
                }
            }
            else if (GamePort == languages.GameClientTypeMStr && Name != null)
            {
                RegistryControl registryControl = new();
                string userdata = registryControl.GetFromRegedit(Name, "Global");
                try
                {
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
                }
                catch
                {
                    MessageBox.Show($"Error : {Name}");
                }
            }
            else
            {
                MessageBox.Show(languages.AddUsersErrorMessageStr);
            }
        }
    }
}
