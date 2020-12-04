using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WinAVFS.Utils;
using WWinAVFS.ContextMenu;

/// <summary>
/// 托管代码不适合编写 COM, .NET Framework is high-impact. COM 组件应是本地代码, 资源占用少, 性能也高.
/// 所以不使用 SharpShell, 直接写 Registry.
/// </summary>
namespace WinAVFS.ContextMenu
{
    public static class Program
    {

        public static string WinAVFSCLIPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAVFS.CLI.exe");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 无参数时, 注册右键菜单界面; 含参数时, 将其作为 压缩文件路径 进行挂载
            // MessageBox.Show(args.Length.ToString());
            if (args.Length == 0)
            {
                //  Application.Run(new RegisteryDialog;
                new RegisteryDialog().ShowDialog();
            }
            else
            {
                if (ArchiveUtils.IsEncrypted(args[0]))
                {
                    new PasswordDialog(args[0]).ShowDialog();
                }
                else
                {
                    /*
                    [lyne]
                    .NET FrameWork 的 ProcessStartInfo.Arguments 是 string, 设计上以空格分隔参数.
                    如果参数包含空格, .NET FrameWork 不会自动在两边添加 "", 需自动添加.
                    .NET 5 的 ProcessStartInfo.ArgumentList 设计上好些.
                    */
                    string pathArg = $"\"path={args[0]}\"";
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = Program.WinAVFSCLIPath,
                        Arguments = pathArg,
                    };
                    Process.Start(startInfo);
                }
                // [lyne] Forms 下执行 Console.WriteLine("无密码"); 无响应.
            }

        }
    }
}
