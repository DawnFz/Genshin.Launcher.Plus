using GenShin_Launcher_Plus.ViewModels;

namespace GenShin_Launcher_Plus.Service.IService
{
    public interface IMainWindowService
    {
        void CheckUpdate(MainWindow main);

        void CheckConfig(MainWindow main);

        void MainBackgroundLoad(MainWindowViewModel vm);
    }
}
