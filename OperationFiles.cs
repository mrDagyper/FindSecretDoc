using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            MatchingName matching = new MatchingName(searchPattern);
            foreach (var filePath in DirectionPath)
            {
                if (filePath.Exists)
                {
                    if (matching.FindMatchingInName(filePath.FullName))
                        matchFiles = matchFiles.Concat(new[] { filePath });


                    if (filePath.Extension == ".docx")
                    {
                        using (MatchingContent matchingContent = new MatchingContent(filePath.FullName, searchPattern))
                        {
                            if (matchingContent.MatchingContentForDocx())
                                matchFiles = matchFiles.Concat(new[] { filePath });
                        }
                    }
                    else
                    {
                        if (MatchingContent(filePath.FullName, searchPattern) != null)
                            matchFiles = matchFiles.Concat(new[] { filePath });
                    }

                }
            }
            return matchFiles;
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
                    string content = System.IO.File.ReadAllText(fileName);
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
