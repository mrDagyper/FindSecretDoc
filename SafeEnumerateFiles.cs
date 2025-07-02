using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FindSecretDoc
{
    internal static class SafeEnumerateFiles
    {
        static public int LeghtConsole = 0;
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo target, string pattern = "*.*")
        {
            try
            {
                var allFiles = Enumerable.Empty<FileInfo>();
                var files = Enumerable.Empty<FileInfo>();
                var stack = new Stack<DirectoryInfo>();
                stack.Push(target);

            while (stack.Any())
            {
                var current = stack.Pop();
                files = OperationFiles.GetAllFiles(GetFiles(current, pattern).Where(x => x.Extension.ToLower() == ".txt"));
                if (files.Any())
                    {
                        allFiles = allFiles.Concat(files);
                    }

                foreach (var subdirectory in GetSubdirectories(current))
                {
                   
                    Console.SetCursorPosition(0, 3);
                    Console.Write(new string(' ', LeghtConsole));
                    Console.SetCursorPosition(0, 3);
                    Console.Write(subdirectory.FullName);
                    LeghtConsole = subdirectory.FullName.Length;
                    stack.Push(subdirectory);
                }

            }
                return allFiles;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory, string pattern)
        {
            try
            {
                return directory.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<FileInfo>();
            }
            
        }

        private static IEnumerable<DirectoryInfo> GetSubdirectories(DirectoryInfo directory)
        {
            try
            {
                return directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<DirectoryInfo>();
            }
        }
    }
}
