using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.Input;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class GuidePageViewModel : ObservableObject
    {
        private IDialogCoordinator dialogCoordinator;
        public GuidePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            DirchooseCommand = new RelayCommand(Dirchoose);
        }
        private string _GamePath;
        public string GamePath
        {
            get=> _GamePath;
            set=> SetProperty(ref _GamePath, value);
        }

        public LanguagesModel languages { get => MainBase.lang; }
        public ICommand DirchooseCommand { get; set; }
        private void Dirchoose()
        {
            CommonOpenFileDialog dialog = new(MainBase.lang.GameDirMsg);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GamePath = dialog.FileName;
                if (!File.Exists(Path.Combine(GamePath, "Yuanshen.exe")) && !File.Exists(Path.Combine(GamePath, "GenshinImpact.exe")))
                {
                    dialogCoordinator.ShowMessageAsync(this, languages.Error, languages.PathErrorMessageStr, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = languages.Determine });
                }
                else
                {
                    MainBase.IniModel.GamePath = GamePath;
                    MainBase.IniModel = new();
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = mainWindow;
                }
            }
        }
    }
}
