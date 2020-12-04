using SevenZip;
using System;
using System.IO;



/// <summary>
///  [requirement] sevenzipsharp, 7z.libs
/// </summary>

namespace WinAVFS.Utils
{
   public class ArchiveUtils
    {
        public static bool IsEncrypted(string path)
        {

            // [lyne] System.AppDomain.CurrentDomain.BaseDirectory 是源程序所在目录.
            // System.Environment.CurrentDirectory 获取和设置当前目录(该进程从中启动的目录)的完全限定目录, 拖动文件到 CLI 程序上, 便是这个.
            string programDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // [lyne] 适用于 x86, x64 场合
            // [Lyne] 判断当前进程是不是 64 bit: 1. NET 4.0+, System.Environment.Is64BitProcess 2. IntPtr.Size == 4 为 32 bit, IntPtr.Size == 8 为 64bit.
            if (Environment.Is64BitProcess)
            {
                SevenZipCompressor.SetLibraryPath(Path.Combine(programDirectory, @"x64\7z.dll"));
            }
            else
            {
                SevenZipCompressor.SetLibraryPath(Path.Combine(programDirectory, @"x86\7z.dll"));
            }

            using (var extractor = new SevenZipExtractor(path))
            {
                // [Lyne] 判断是否加密
                // "extractor.Check() = false" means it's encrypted.
                return !extractor.Check();
            }
        }
    }
}
