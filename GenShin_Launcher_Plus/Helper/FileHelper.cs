using GenShin_Launcher_Plus.Core.Extension;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Runtime.InteropServices;

namespace GenShin_Launcher_Plus.Core
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


        /*
        public static bool UnZip(string zipFile, string directory)
        {
            try
            {
                ZipInputStream f = new(File.OpenRead(zipFile));
            A: ZipEntry zp = f.GetNextEntry();
                while (zp != null)
                {
                    string un_tmp2;
                    if (zp.Name.EndsWith("/"))
                    {
                        int tmp1 = zp.Name.LastIndexOf("/");
                        un_tmp2 = zp.Name.Substring(0, tmp1);
                        Directory.CreateDirectory(directory + un_tmp2);
                    }
                    if (!zp.IsDirectory && zp.Crc != 00000000L)
                    {
                        int i = 2048;
                        byte[] b = new byte[i];
                        FileStream s = File.Create(directory + zp.Name);
                        while (true)
                        {
                            i = f.Read(b, 0, b.Length);
                            if (i > 0)
                                s.Write(b, 0, i);
                            else
                                break;
                        }
                        s.Close();
                    }
                    goto A;
                }
                f.Close();
                return true;
            }
            catch { return false; }
        }*/
    }
}
