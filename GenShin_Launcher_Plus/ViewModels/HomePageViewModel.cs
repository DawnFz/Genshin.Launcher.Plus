using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenShin_Launcher_Plus.Models;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;

using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// 启动页的ViewModel
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
            if (App.Current.DataModel.SwitchUser != null && App.Current.DataModel.SwitchUser != "")
            {
                App.Current.NoticeOverAllBase.IsSwitchUser = "Visible";
                App.Current.NoticeOverAllBase.SwitchUser = $"{languages.UserNameLab} : {App.Current.DataModel.SwitchUser}";
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
