using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindSecretDoc
{
    internal class OperationFiles
    {
        public static string searchPattern;

        /// <summary>
        /// Возвращает список файлов подходящих по шаблону
        /// </summary>
        /// <param name="DirectionPath">Директория</param>
        /// <param name="searchPattern">шаблон</param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetAllFiles(IEnumerable<FileInfo> DirectionPath)
        {
            var matchFiles = Enumerable.Empty<FileInfo>();

            foreach (var filePath in DirectionPath)
            {
                if (MatchingName(filePath.FullName, searchPattern) != null)
                    matchFiles = matchFiles.Concat(new[] { filePath });

                if (MatchingContent(filePath.FullName, searchPattern) != null)
                    matchFiles = matchFiles.Concat(new[] { filePath });
            }
            return matchFiles;
        }

        /// <summary>
        /// Поиск соответствий в названии файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        private static string MatchingName(string path, string searchPattern)
        {
            if (!string.IsNullOrWhiteSpace(searchPattern))
            {
                try
                {
                    bool nameMatch = path.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (nameMatch)
                        return path;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Поиск соответствий в содержании файла
        /// </summary>
        /// <param name="fileName">директория файла</param>
        /// <param name="searchPattern">шаблон</param>
        /// <returns></returns>
        private static string MatchingContent(string fileName, string searchPattern)
        {
            if (!string.IsNullOrWhiteSpace(searchPattern))
            {
                try
                {
                    string content = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
                    bool contentMatch = content.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (contentMatch)
                        return fileName;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
