using System;
using System.Collections.Concurrent;
using WinAVFS.Core;
using WinAVFS.Utils;

namespace WinAVFS.CLI
{
    public class WinAVFSCLI
    {
        public static void Main(string[] args)
        {
            HandleStringArguments(args);
        }

        public static void ShowUsage()
        {
            Console.WriteLine(@"Usage: WinAVFS.CLI.exe <path to archive> [password] [mount point]");
            Console.WriteLine(@"Example: WinAVFS.CLI.exe ""path=D:\test.7z"" ""password=0123"" ""mount_point=D:\test""");
        }


        public static void HandleStringArguments(String[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }
            ConcurrentDictionary<String, String> argDict = ArgumentUtils.Parse(args);

            // [lyne] Console.WriteLine() 可能会一闪而过, 用  MessageBox.Show() 就可以停留. 注意, CLI 程序应移除 System.Windows.Forms.dll 引用.
            // Console.WriteLine(argDict.Keys.ToString());

            /* 
            [lyne] 
            以程序的逻辑 (功能职责) 来设计代码利于优化, 以人的逻辑 (关注点) 来设计代码不利优化, 何况人的思维方式总是不一样的.
            比如此处, 从人思维角度设计:
                人关心的是传的参数个数. 所以先判断是否传了一个参数, 再判断这个参数是不是符合格式的 "path=<路径>", 怎么判断? ArgumentUtils.Parse(String[]) 解析不到 "path=<路径>".
                略过 n 个步骤, 确实繁琐.

            而已代码逻辑角度设计:
                方案一:     
                ArgumentUtils.Parse(String[]) 只负责解析符合格式的参数, 不管多少个参数, 只要没有解析到 "path=<路径>".
                就当做只传一个参数处理, 若此时传入多个直接报错.

                方案二:
                因为最终的参数解析功能依赖于 HandleDictionaryArguments(), 所以处理生成 HandleDictionaryArguments() 的参数
                

             */



            /*
             没有 传入符合格式 "参数名=参数值" 的参数时, {argDict} = null.
             如直接把文件拖到 CLI 程序上, 此时没有使用 path 参数, 仅有一个纯路径, 则将其作为 path 参数

             传入符合格式 "参数名=参数值" 的参数时, {argDict} 不为空, 但可能没有传入 path 参数.
            */
            // 没有解析到 "path=<路径>", 则只能出现一个参数
            if (argDict == null || !argDict.ContainsKey("path"))
            {
                if (args.Length == 1)
                {
                    if (argDict == null)
                    {
                        argDict = new ConcurrentDictionary<string, string>();
                    }
                    argDict.TryAdd("path", args[0]);
                }
                // 如有 2+ 个参数, 则必须包含 path 参数, 否则直接报错
                else
                {
                    Console.WriteLine("[ERROR] Wrong arguments.");
                    ShowUsage();
                    return;
                }
            }

            HandleDictionaryArguments(argDict);
        }

        /// <summary>
        ///     1. Only one argument <path> supported by use the default config mount point.
        ///     2. Support password.
        /// </summary>
        /// <param name="argDict"></param>
        public static void HandleDictionaryArguments(ConcurrentDictionary<String, String> argDict)
        {

            if (argDict == null)
            {
                Console.WriteLine("[Warning] No arguments. Do nothing.");
                return;
            }

            ReadOnlyAVFS fs = null;

            string outPathValue = "";
            if (!argDict.ContainsKey("path"))
            {
                Console.WriteLine("[ERROR] You must specify the <path> argument.");
                ShowUsage();
                return;
            }
            else if (argDict.TryGetValue("path", out outPathValue) && outPathValue == "")
            {
                Console.WriteLine(@"[ERROR] The <path> argument is """".");
                return;
            }

            string outPasswordValue = "";
            if (!argDict.ContainsKey("password"))
            {
                fs = new ReadOnlyAVFS(new SevenZipProvider(outPathValue));
            }
            // ignore null password
            else if (argDict.TryGetValue("password", out outPasswordValue) && outPasswordValue != "")
            {
                fs = new ReadOnlyAVFS(new SevenZipProvider(outPathValue, outPasswordValue));
            }

            string outMountPointValue = "";
            if (!argDict.ContainsKey("mount_point"))
            {
                string defaultMountPoint = FileSystemUtils.GetEmptyDirectory(outPathValue, "_[WinAVFS]");
                fs.Mount(defaultMountPoint);
                Console.WriteLine("[Info] Mounted:", defaultMountPoint);
            }
            else if (argDict.TryGetValue("mount_point", out outMountPointValue))
            {
                fs.Mount(outMountPointValue);
                Console.WriteLine("[Info] Mounted:", outMountPointValue);
            }
        }
    }

}