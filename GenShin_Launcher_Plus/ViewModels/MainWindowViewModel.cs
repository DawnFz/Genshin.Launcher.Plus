using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using GenShin_Launcher_Plus.Models;
using GenShin_Launcher_Plus.Service.IService;
using GenShin_Launcher_Plus.Service;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private IDialogCoordinator dialogCoordinator;
        public MainWindowViewModel(IDialogCoordinator instance, MainWindow main)
        {
            dialogCoordinator = instance;
            App.Current.LoadProgramCore.LoadLanguageCore();
            new UpdateService().CheckUpdate(main);
            MainService = new MainService(main,this);

            OpenImagesDirectoryCommand = new RelayCommand(OpenImagesDirectory);
            OpenAboutCommand = new RelayCommand(OpenAbout);
            OpenQQGroupUrlCommand = new RelayCommand(OpenQQGroupUrl);
            ExitProgramCommand = new RelayCommand(ExitProgram);
            MainMinimizedCommand = new RelayCommand(MainMinimized);

            Title = $"{languages.MainTitle} {Application.ResourceAssembly.GetName().Version}";
            App.Current.IniModel.EXEname(Path.GetFileName(Environment.ProcessPath));
        }

        public IMainWindowService MainService { get; set; }

        public LanguageModel languages { get => App.Current.Language; }
        public string Title { get; set; }
        private ImageBrush _Background;
        public ImageBrush Background { get => _Background; set => SetProperty(ref _Background, value); }

        public ICommand OpenImagesDirectoryCommand { get; set; }
        private async void OpenImagesDirectory()
        {
            if (Directory.Exists(Path.Combine(App.Current.IniModel.GamePath, "ScreenShot")))
            {
                ProcessStartInfo info = new()
                {
                    FileName = Path.Combine(App.Current.IniModel.GamePath, "ScreenShot"),
                    UseShellExecute = true,
                };
                Process.Start(info);
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(
                    this, languages.Error,
                    languages.ScreenPathErr,
                    MessageDialogStyle.Affirmative,
                    new MetroDialogSettings() 
                    { AffirmativeButtonText = languages.Determine });
            }
        }

        public ICommand OpenAboutCommand { get; set; }
        private async void OpenAbout()
        {
            if ((await dialogCoordinator.ShowMessageAsync(
                this, languages.AboutTitle, 
                languages.AboutStr,
                MessageDialogStyle.AffirmativeAndNegative, 
                new MetroDialogSettings()
                { 
                    AffirmativeButtonText = languages.Determine,
                    NegativeButtonText = "GitHub" 
                })) != MessageDialogResult.Affirmative)
            {
                ProcessStartInfo info = new()
                {
                    FileName = "https://github.com/DawnFz/Genshin.Launcher.Plus",
                    UseShellExecute = true,
                };
                Process.Start(info);
            }
        }

        public ICommand OpenQQGroupUrlCommand { get; set; }
        private void OpenQQGroupUrl()
        {
            ProcessStartInfo info = new()
            {
                FileName = "https://jq.qq.com/?_wv=1027&k=Kxt00f0Y",
                UseShellExecute = true,
            };
            Process.Start(info);
        }

        public ICommand ExitProgramCommand { get; set; }
        private void ExitProgram()
        {
            Environment.Exit(0);
        }

        public ICommand MainMinimizedCommand { get; set; }
        private void MainMinimized()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }
    }
}
