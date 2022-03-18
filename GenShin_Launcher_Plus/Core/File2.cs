using System.IO;

namespace GenShin_Launcher_Plus.Core
{
    internal class File2
    {
        /// <summary>
        /// 文件是否被打开
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns></returns>
        public static bool IsFileOpen(string path)
        {
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
