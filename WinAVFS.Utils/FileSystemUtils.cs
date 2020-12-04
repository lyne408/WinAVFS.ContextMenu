using System;
using System.IO;

namespace WinAVFS.Utils
{
    public class FileSystemUtils
    {
        /// <summary>
        ///     DeepAQ 开发的 WinAVFS 不会自动创建文件夹, 因为 mount 后没有入口文件夹.
        ///     GetEmptyDirectory() 函数会自动创建 空文件夹.
        ///     
        ///     例如:
        ///         ObtainEmptyDirectory("D:\test.7z", "_[WinAVFS]") 会自动创建空文件夹 "D:\test.7z_[WinAVFS]".
        ///         若 "D:\test.7z_[WinAVFS]" 非空, 则会添加毫秒级的当前时间后缀, 会新建 "D:\test.7z_[WinAVFS]_2020-11-25_12-45-09_555".
        ///         
        ///     为何需要第二个参数 suffix? 建议 suffix 设为当前程序的标识, 让用户清楚该空文件夹是何程序所建. 
        /// </summary>
        /// <param name="basePath">基础路径</param>
        /// <param name="suffix">默认后缀</param>
        /// <returns>新建的空文件的路径</returns>
        public static string GetEmptyDirectory(string basePath, string suffix)
        {
            string path = basePath + suffix;

            // If not exists, create directory then mount.
            if (!IsExists(path))
            {
                Directory.CreateDirectory(path);
            }
            // If exists empty directory, then mount, so not need clause.
            // If existent directory is not empty, create other directory then mount.
            else if (!IsEmptyDirectory(path))
            {
                path = basePath + suffix + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
                Directory.CreateDirectory(path);
            }
            return path;
        }


        /// <summary>
        ///     Determine whether the given path on disk exists. 
        /// </summary>
        /// <param name="path">The given path.</param>
        /// <returns>Exist or not.</returns>
        public static bool IsExists(string path)
        {
            // is a file
            if (File.Exists(path))
            {
                return true;
            }
            // is a directory
            else if (Directory.Exists(path))
            {
                return true;
            }
            // neither, throw error
            else
            {
                return false;
            }

        }

        public static bool IsDirectory(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
                return true;
            else
                return false;
        }

        public static bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        /// <summary>
        ///     Determine whether a directory is empty or not.
        ///     [Requirement] The given path already exists.
        ///     If it's a file or a directory contains some files, return false.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string path)
        {
            if (!IsDirectory(path))
            {
                return false;
            }
            else
            {
                string[] files = Directory.GetFiles(path);
                return files.Length == 0;
            }
        }
    }
}
