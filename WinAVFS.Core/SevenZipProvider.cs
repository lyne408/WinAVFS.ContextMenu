using System;
using System.IO;
using System.Runtime.InteropServices;
using SevenZip;

namespace WinAVFS.Core
{
    public class SevenZipProvider : IArchiveProvider
    {
        private readonly ConcurrentObjectPool<SevenZipExtractor> extractorPool;
        /// <summary>
        /// Add by lyne408
        /// [requirement] 7z.libs
        /// </summary>
        static SevenZipProvider() {
            string programDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (Environment.Is64BitProcess)
            {
                SevenZipExtractor.SetLibraryPath(Path.Combine(programDirectory, @"x64\7z.dll"));
            }
            else
            {
                SevenZipExtractor.SetLibraryPath(Path.Combine(programDirectory, @"x86\7z.dll"));
            }
        }

        public SevenZipProvider(string path)
        {
            Console.WriteLine($"Loading archive {path}");
            this.extractorPool = new ConcurrentObjectPool<SevenZipExtractor>(() => new SevenZipExtractor(path));
        }

        /// <summary>
        ///     Constructor added by lyne408 to support password.
        /// </summary>
        /// <param name="path">Archive file path.</param>
        /// <param name="password">Password of that archive.</param>
        public SevenZipProvider(string path, string password)
        {
            Console.WriteLine($"Loading archive {path}, using password");
            this.extractorPool = new ConcurrentObjectPool<SevenZipExtractor>(() => new SevenZipExtractor(path, password));
        }


        public void Dispose()
        {
            foreach (var archive in this.extractorPool.GetAll())
            {
                archive.Dispose();
            }
        }

        public FSTree ReadFSTree()
        {
            var extractor = this.extractorPool.Get();
            var root = new FSTreeNode(true);
            foreach (var entry in extractor.ArchiveFileData)
            {
                // Console.WriteLine($"Loading {entry.FileName} into FS tree");
                var paths = entry.FileName.Split('/', '\\');
                var node = root;
                for (var i = 0; i < paths.Length - 1; i++)
                {
                    node = node.GetOrAddChild(true, paths[i]);
                }

                if (!string.IsNullOrEmpty(paths[paths.Length - 1]))
                {
                    node = node.GetOrAddChild(entry.IsDirectory, paths[paths.Length - 1], (long)entry.Size,
                        (long)entry.Size, entry.Index);
                    node.CreationTime = entry.CreationTime;
                    node.LastAccessTime = entry.LastAccessTime;
                    node.LastWriteTime = entry.LastWriteTime;
                    // if (!node.IsDirectory && node.Buffer == IntPtr.Zero)
                    // {
                    //     node.Buffer = Marshal.AllocHGlobal((IntPtr) node.Length);
                    // }
                }
            }

            Console.WriteLine($"Loaded {extractor.FilesCount} entries from archive");
            this.extractorPool.Put(extractor);
            return new FSTree { Root = root };
        }

        public void ExtractFileUnmanaged(FSTreeNode node, IntPtr buffer)
        {
            if (!(node.Context is int index))
            {
                throw new ArgumentException();
            }

            unsafe
            {
                using var target = new UnmanagedMemoryStream((byte*)buffer.ToPointer(), node.Length, node.Length,
                    FileAccess.Write);
                var extractor = this.extractorPool.Get();
                extractor.ExtractFile(index, target);
                this.extractorPool.Put(extractor);
            }
        }
    }
}