namespace GenShin_Launcher_Plus.Models
{
    public class UserListModel
    {
        public string UserName { get; set; }
    }
    public class DisplaySizeListModel
    {
        public string SizeName { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public bool IsNull { get; set; }
    }
    public class GamePortListModel
    {
        public string GamePort { get; set; }
    }
    public class GameWindowModeListModel
    {
        public string GameWindowMode { get; set; }
    }
    public class LanguageListModel
    {
        public string LangID { get; set; }
        public string LangVersion { get; set; }
        public string LangName { get; set; }
        public string LangFileName { get; set; }
    }
}
