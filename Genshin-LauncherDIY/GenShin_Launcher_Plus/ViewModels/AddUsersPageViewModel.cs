using GenShin_Launcher_Plus.Command;
using GenShin_Launcher_Plus.Core;
using GenShin_Launcher_Plus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class AddUsersPageViewModel : NotificationObject
    {

        public AddUsersPageViewModel()
        {
            CreateGamePortList();
            SaveUserDataCommand = new DelegateCommand { ExecuteAction = new Action<object>(SaveUserData) };
        }

        public string GamePort { get; set; }
        public string Name { get; set; }

        //游戏客户端列表
        private List<GamePortListModel> _GamePortLists;
        public List<GamePortListModel> GamePortLists
        {
            get { return _GamePortLists; }
            set
            {
                _GamePortLists = value;
                OnPropChanged("GamePortLists");
            }
        }
        private void CreateGamePortList()
        {
            GamePortLists = new();
            GamePortLists.Add(new GamePortListModel { GamePort = "国服" });
            GamePortLists.Add(new GamePortListModel { GamePort = "国际" });
        }

        public DelegateCommand SaveUserDataCommand { get; set; }
        private void SaveUserData(object parameter)
        {
            if (GamePort == "国服" && Name != null)
            {
                RegistryControl registryControl = new();
                string userdata = registryControl.GetFromRegedit(Name, "CN");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else if (GamePort == "国际" && Name != null)
            {
                RegistryControl registryControl = new();
                string userdata = registryControl.GetFromRegedit(Name, "Global");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            }
            else
            {
                MessageBox.Show("未选择账号所属服务器或未输入保存的名称，本次保存不生效！");
            }
        }
    }
}
