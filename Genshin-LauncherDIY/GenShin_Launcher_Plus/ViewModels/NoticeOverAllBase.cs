using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class NoticeOverAllBase : ObservableObject
    {
        public NoticeOverAllBase()
        {

        }
        //贯通到主页面的索引
        private int _MainPagesIndex;
        public int MainPagesIndex
        {
            get => _MainPagesIndex;
            set => SetProperty(ref _MainPagesIndex, value);
        }
        //Home页面显示label

        private string _SwitchUser;
        public string SwitchUser
        {
            get => _SwitchUser;
            set => SetProperty(ref _SwitchUser, value);
        }

        private string _SwitchPort;
        public string SwitchPort
        {
            get => _SwitchPort;
            set => SetProperty(ref _SwitchPort, value);
        }

        private string _IsSwitchUser;
        public string IsSwitchUser
        {
            get => _IsSwitchUser;
            set => SetProperty(ref _IsSwitchUser, value);
        }
    }
}
