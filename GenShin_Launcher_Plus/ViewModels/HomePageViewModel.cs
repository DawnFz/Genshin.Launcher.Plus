using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;

using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 这个类为启动页的ViewModel
    /// </summary>
    public class HomePageViewModel : ObservableObject
    {
        private ILaunchService LaunchService { get; set; }
        private IDialogCoordinator dialogCoordinator;
        public HomePageViewModel(IDialogCoordinator instance)
        {
            LaunchService = new LaunchService(instance);
            dialogCoordinator = instance;
            RunGameCommand = new AsyncRelayCommand(LaunchService.RunGameAsync);
            if (App.Current.IniModel.SwitchUser != null && App.Current.IniModel.SwitchUser != "")
            {
                App.Current.NoticeOverAllBase.IsSwitchUser = "Visible";
                App.Current.NoticeOverAllBase.SwitchUser = $"{languages.UserNameLab} : {App.Current.IniModel.SwitchUser}";
            }
            else
            {
                App.Current.NoticeOverAllBase.IsSwitchUser = "Hidden";
            }
        }
        public LanguageModel languages { get => App.Current.Language; }

        public ICommand RunGameCommand { get; set; }
    }
}
