using GenShin_Launcher_Plus.Core.Extension;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Runtime.InteropServices;

namespace GenShin_Launcher_Plus.Helper
{
    internal class FileHelper
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        /// <summary>
        /// 文件是否被打开
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns></returns>
        public bool IsFileOpen(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            IntPtr vHandle = _lopen(path, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {               
                return true;
            }
            CloseHandle(vHandle);
            return false;
        }

        /// <summary>
        /// 将程序中的资源文件写出到硬盘
        /// </summary>
        public static void ExtractEmbededAppResource(string resourceName, string fileName)
        {
            Uri uri = new($"pack://application:,,,/{resourceName}", UriKind.RelativeOrAbsolute);
            Stream stream = Application.GetResourceStream(uri).Stream;
            stream.ToFile(fileName);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool UnZip(string fileName)
        {
            try
            {
                ZipFile.ExtractToDirectory($"{Environment.CurrentDirectory}/{fileName}", Environment.CurrentDirectory, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
