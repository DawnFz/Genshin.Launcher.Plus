namespace GenShin_Launcher_Plus.Models
{
    public class UpdateModel
    {
        public string Version { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DownloadUrl { get; set; }
        public string GlobalDownloadUrl { get; set; }
        public string PkgVersion { get; set; }
        public string BgUrl { get; set; }
        public bool RequisiteUpdate { get; set; }

    }
}
