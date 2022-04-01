using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// UsersPage的ViewModel 
    /// 集成了UsersPage的部分操作实现逻辑
    /// </summary>
    public class UsersPageViewModel : ObservableObject
    {

        public UsersPageViewModel()
        {
            _GamePortLists = new();

            SaveUserDataCommand = new RelayCommand(SaveUserData);
            RemoveThisPageCommand = new RelayCommand(RemoveThisPage);

            _registryService = new RegistryService();
            _userDataService = new UserDataService();

            GamePortLists
                .Add(new GamePortListModel
                { GamePort = languages.GameClientTypePStr });
            GamePortLists
                .Add(new GamePortListModel
                { GamePort = languages.GameClientTypeMStr });
        }

        private IRegistryService _registryService;
        public IRegistryService RegistryService { get => _registryService; }

        private IUserDataService _userDataService;
        public IUserDataService UserDataService { get => _userDataService; }

        public LanguageModel languages { get => App.Current.Language; }

        public string GamePort { get; set; }
        public string Name { get; set; }

        //游戏客户端列表
        private List<GamePortListModel> _GamePortLists;
        public List<GamePortListModel> GamePortLists { get => _GamePortLists; }

        public ICommand SaveUserDataCommand { get; set; }
        private void SaveUserData()
        {
            if (GamePort == languages.GameClientTypePStr && Name != null)
            {
                string userdata = RegistryService.GetFromRegistry(Name, "CN");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else if (GamePort == languages.GameClientTypeMStr && Name != null)
            {
                string userdata = RegistryService.GetFromRegistry(Name, "Global");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else
            {
                MessageBox.Show(languages.AddUsersErrorMessageStr);
            }
            App.Current.NoticeOverAllBase.UserLists = UserDataService.ReadUserList();
            RemoveThisPage();
        }

        public ICommand RemoveThisPageCommand { get; set; }
        private void RemoveThisPage()
        {
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
        }
    }
}
