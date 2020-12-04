using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAVFS.Utils
{
    /// <summary>
    /// class ShellItemRegistry
    /// 
    /// <strong>
    ///     使用 注册 "扩展名\shell\ShellItem" 来创建右键菜单, 仅 "*"(所有文件) 和 "Directory"(文件夹) 一定会生效, 其余未必.
    ///     要想任意扩展名类型的右键菜单生效, 需要使用 COM.
    /// </strong>
    /// 
    /// User Guide:
    /// 
    ///     注册右键菜单, 用户最关心且不可少的参数是:
    ///         1. 关联的类型
    ///         2. 右键菜单项的文字
    ///         3. 可执行文件的路径
    ///     
    ///     使用: 创建实例, 设置3个必须的属性, 调用所需的 注册方法 或 清除方法
    /// </summary>

    public class ShellItemRegistry
    {

        /// <summary>
        /// 右键菜单项的文字
        /// </summary>
        public string MenuText;
        /// <summary>
        /// 可执行文件的路径
        /// </summary>
        public string ExecutableFilePath;
        /// <summary>
        /// 关联的类型数组
        /// </summary>
        public string[] Associations;
        /// <summary>
        /// 可不设置. 
        /// 不设置时, 自动设置 ExecutableFilePath 的值.
        /// </summary>
        public string IconPath;
        /// <summary>
        /// 可不设置.
        /// 不设置时, 自动设置 MenuText 的值, 取代其中空格为下划线.
        /// </summary>
        public string ItemName;
        

        private void InnerRegister(string shellKeyLocation)
        {
            RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(shellKeyLocation, true);
            if (shellKey == null)
            {
                shellKey = Registry.ClassesRoot.CreateSubKey(shellKeyLocation);
            }
            if (this.ItemName == String.Empty) {
                this.ItemName = this.MenuText.Replace(" ", "_");
            }
            RegistryKey shellItemKey = shellKey.CreateSubKey(this.ItemName);
            shellItemKey.SetValue(string.Empty, this.MenuText);
            if (this.IconPath == String.Empty)
            {
                this.IconPath = this.ExecutableFilePath;
            }
            shellItemKey.SetValue("Icon", this.IconPath);
            RegistryKey commandKey = shellItemKey.CreateSubKey("command");

            /*
             like default value of Computer\HKEY_CLASSES_ROOT\*\shell\VSCode\command:
                "C:\Program Files\Microsoft VS Code\Code.exe" "%1"
            */
            commandKey.SetValue(string.Empty, $"\"{this.ExecutableFilePath}\" %1");

            commandKey.Close();
            shellItemKey.Close();
            shellKey.Close();
        }

        private void InnerUnegister(string shellKeyLocation)
        {
            RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(shellKeyLocation, true);
            if (shellKey == null)
            {
                return;
            }
            else
            {
                RegistryKey shellItemKey = shellKey.OpenSubKey(this.ItemName, true);
                if (shellItemKey == null)
                {
                    shellKey.Close();
                    return;
                }
                else
                {
                    shellKey.DeleteSubKeyTree(this.ItemName);
                    shellItemKey.Close();
                    shellKey.Close();
                }
            }
        }

        public void Register()
        {
            foreach (string association in this.Associations)
            {
                InnerRegister($"{association}\\shell");
            }
        }

        public void RegisterAllFiles()
        {
            InnerRegister(@"*\shell");
        }

        public void RegisterDirectory()
        {
            InnerRegister(@"directory\shell");
        }

        public void Unregister()
        {
            foreach (string association in this.Associations)
            {
                InnerUnegister($"{association}\\shell");
            }
        }

        public void UnregisterAllFiles()
        {
            InnerUnegister(@"*\shell");
        }

        public void UnregisterDirectory()
        {
            InnerUnegister(@"directory\shell");
        }
    }
}
