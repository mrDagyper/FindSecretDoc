using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindSecretDoc
{
    internal static class SafeEnumerateFiles
    {
        public static int LeghtConsole = 0;
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo target, string pattern = "*.*")
        {
                var allFiles = Enumerable.Empty<FileInfo>();
            try
            {
                var files = Enumerable.Empty<FileInfo>();
                var stack = new Stack<DirectoryInfo>();
                stack.Push(target);

                while (stack.Any())
                {

                    try
                    {
                        var current = stack.Pop();

                        if (current.FullName.Length < 248)
                        {
                            files = OperationFiles.GetAllFiles(GetFiles(current, pattern));
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
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                return allFiles;
            }
            catch (Exception ex)
            {

                //Console.WriteLine(ex.Message);
                return allFiles;
                //throw;
            }

        }

        private static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory, string pattern)
        {
            try
            {
                return directory.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly).Where(x => (x.FullName.Length < 260) &
                                                                                               ((x.Extension == ".txt") ||
                                                                                               (x.Extension.ToLower() == ".doc") ||
                                                                                                (x.Extension.ToLower() == ".docx") ||
                                                                                                (x.Extension.ToLower() == ".pdf") ||
                                                                                                /*(x.Extension.ToLower() == ".zip") ||
                                                                                                (x.Extension.ToLower() == ".rar") ||
                                                                                                (x.Extension.ToLower() == ".7z") ||*/
                                                                                                // (x.Extension.ToLower() == ".xml") || Раскоментить если есть в этом необходимость
                                                                                                (x.Extension.ToLower() == ".rtf")));
            }
            catch (UnauthorizedAccessException)
            {
                return Enumerable.Empty<FileInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
