using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAVFS.CLI
{
    public class Arguments
    {
        public string Path;
        public string Password;
        public string MountPoint;
        /// <summary>
        /// "默认" 思想.
        /// 目前可自动根据 Path 设置 MountPoint
        /// </summary>
        private void AutoSetProperty() { 
        
        }
        public Arguments(string path) {
            this.Path = path;
        }
        public Arguments(string path, string password)
        {
            this.Path = path;
            this.Password = password;
        }
        public Arguments(string path, string password, string mountPoint)
        {
            this.Path = path;
            this.Password = password;
            this.MountPoint = mountPoint;
        }
    }
}
