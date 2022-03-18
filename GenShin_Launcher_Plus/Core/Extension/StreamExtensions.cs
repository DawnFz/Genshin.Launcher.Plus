using System;
using System.IO;

namespace GenShin_Launcher_Plus.Core.Extension
{
    internal static class StreamExtensions
    {
        public static void ToFile(this Stream stream, string fileName)
        {
            if (stream.CanSeek && stream.CanRead)
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);

                using (FileStream fileStream = new(fileName, FileMode.Create))
                {
                    using (BinaryWriter binaryWriter = new(fileStream))
                    {
                        binaryWriter.Write(bytes);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("流无法定位与读取，写入文件操作已取消");
            }
        }
    }

}
