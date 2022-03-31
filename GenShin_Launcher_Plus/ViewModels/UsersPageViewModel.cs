using GenShin_Launcher_Plus.Service;
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
    public class UsersPageViewModel : ObservableObject
    {

        public UsersPageViewModel()
        {
            CreateGamePortList();
            SaveUserDataCommand = new RelayCommand(SaveUserData);
            RemoveThisPageCommand = new RelayCommand(RemoveThisPage);
        }

        public LanguageModel languages { get => App.Current.Language; }

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
            RegistryService registryControl = new();
            if (GamePort == languages.GameClientTypePStr && Name != null)
            {
                string userdata = registryControl.GetFromRegistry(Name, "CN");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else if (GamePort == languages.GameClientTypeMStr && Name != null)
            {
                string userdata = registryControl.GetFromRegistry(Name, "Global");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else
            {
                MessageBox.Show(languages.AddUsersErrorMessageStr);
            }
            RemoveThisPage();
        }

        public ICommand RemoveThisPageCommand { get; set; }
        private void RemoveThisPage()
        {
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
        }
    }
}
