using System;
using System.Collections.Concurrent;

namespace WinAVFS.Utils
{
    public class ArgumentUtils
    {
        /// <summary>
        ///     Arguments Parser.
        ///     
        ///     Use to parse eye-firendly arguments code style, like this:
        ///         WinAVFS.CLI.exe "path=D:\test.7z" "password=0123" "mount_point=D:\test"
        ///         
        ///     注意: 
        ///         1. Return null when not passed arguments.
        ///            没有参数时返回 null
        ///         2. 解析后, 没有符合格式的参数也会返回 null
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<String, String> Parse(string[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }

            // TODO, 应该把参数设计为一个单独的 Class, 而不是 ConcurrentDictionary instance...
            ConcurrentDictionary<String, String> argDict = new ConcurrentDictionary<String, String>();
            foreach (string arg in args)
            {
                // [lyne] 必须确有字符串, Contains("="), 才可根据 IndexOf("=") 截取子串. 否则 IndexOf("=") 为负值, 可能使 Substring(0, fisrtEqualSignIndex) 引发 Length cannot be less than zero 异常.
                if (arg.Contains("=")) {
                    string argTrimed = arg.Trim();
                    int fisrtEqualSignIndex = argTrimed.IndexOf("=");
                    string name = argTrimed.Substring(0, fisrtEqualSignIndex).Trim().ToLower();
                    string value = argTrimed.Substring(fisrtEqualSignIndex + 1).Trim();
                    argDict.TryAdd(name, value);
                }
                
            }

            // [lyne] 如果解析不到任何符合格式的参数, 没有 key-value 对, 空的字典, 显然不必返回, 直接销毁在栈内存. 同理, 如空的数组, 集合等, 都不必返回, 直接返回 null.
            if (argDict.Count == 0)
            {
                return null;
            }
            else {
                return argDict;
            }
            
        }


        /// <summary>
        /// TODO, 泛型的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T ParseGeneric<T>(string[] args)
        {
            ConcurrentDictionary<String, String>  argDict = Parse(args);

            // 如果 T 没有 public constructor, return default(T); 若有则进行 public 属性遍历, 要求 public 属性的首字母大写.

            return default(T);
        }
    }
}
