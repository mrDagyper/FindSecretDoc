using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FindSecretDoc
{
    internal class OperationFiles
    {
        public string[] searchPattern = Settings.SearchPattern;

        /// <summary>
        /// Возвращает список файлов подходящих по шаблону
        /// </summary>
        /// <param name="DirectionPath">Директория</param>
        /// <param name="searchPattern">шаблон</param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetAllFiles(IEnumerable<FileInfo> DirectionPath)
        {
            var matchFiles = Enumerable.Empty<FileInfo>();

            MatchingContent matchingContent = new MatchingContent();
            ExtractText extractText = new ExtractText();
            string[] content = Settings.SearchPattern;

            MatchingName matching = new MatchingName();

            foreach (var filePath in DirectionPath)
            {
                if (filePath.Exists)
                {
                    if (Settings.SearchInName)
                    {
                        var result = matching.FindMatchingInName(filePath.FullName, content);
                        if (result)
                        {
                            //matchFiles = matchFiles.Concat(new[] { filePath });
                            Settings.NameMatches.Add(filePath);
                        }
                    }

                    if (filePath.Extension == ".docx" || filePath.Extension == ".doc")
                    {
                            if (matchingContent.MatchingContentForDocx(filePath.FullName, content))
                                matchFiles = matchFiles.Concat(new[] { filePath });
                    }
                    else if (filePath.Extension == ".pdf")
                    {
                        string text = extractText.MatchingContentForPDF(filePath.FullName);
                        if (text != null)
                        {
                            if (matchingContent.MatchingContentOtherTextPDF(text, content))
                                matchFiles = matchFiles.Concat(new[] { filePath });
                        }
                    }
                    else
                    {
                        if (matchingContent.MatchingContentOtherText(filePath.FullName, content))
                            matchFiles = matchFiles.Concat(new[] { filePath });
                    }

                }
            }

            return matchFiles;
        }
    }
}
