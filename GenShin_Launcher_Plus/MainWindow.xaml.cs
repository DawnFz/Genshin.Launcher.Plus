using GenShin_Launcher_Plus.Helper;
using GenShin_Launcher_Plus.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace GenShin_Launcher_Plus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindowViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            App.Current.ThisMainWindow = this;
            ViewModel = new MainWindowViewModel(DialogCoordinator.Instance, this);
            DataContext = ViewModel;
            MainSizeBinding();
            MainFlipView.DataContext = App.Current.NoticeOverAllBase;
            HomePage.Children.Add(new Views.HomePage());
        }

        private void WindowDragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.Children.Clear();
            SettingsPage.Children.Add(new Views.SettingPage());
            MainFlipView.SelectedIndex = 1;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUsersPage.Children.Clear();
            AddUsersPage.Children.Add(new Views.UsersPage());
            MainFlipView.SelectedIndex = 2;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            FileHelper.OpenUrl("https://www.dawnfz.com/document/");
        }

        private void MainSizeBinding()
        {
            //Height
            Binding mainHeight = new()
            {
                Source = ViewModel,
                Path = new PropertyPath("MainHeight"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            SetBinding(HeightProperty, mainHeight);

            //Width
            Binding mainWidth = new()
            {
                Source = ViewModel,
                Path = new PropertyPath("MainWidth"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            SetBinding(WidthProperty, mainWidth);
        }
    }
}
