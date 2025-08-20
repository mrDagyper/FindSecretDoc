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
            MatchingContent matchingContent = new MatchingContent(searchPattern);
            ExtractText extractText = new ExtractText();

            foreach (var filePath in DirectionPath)
            {
                if (filePath.Exists)
                {
                    if (filePath.Extension == ".docx" || filePath.Extension == ".doc")
                    {
                            if (matchingContent.MatchingContentForDocx(filePath.FullName))
                                matchFiles = matchFiles.Concat(new[] { filePath });
                    }
                    else if (filePath.Extension == ".pdf")
                    {
                        string text = extractText.MatchingContentForPDF(filePath.FullName);
                        if (text != null)
                        {
                            if (matchingContent.MatchingContentOtherTextPDF(text))
                                matchFiles = matchFiles.Concat(new[] { filePath });
                        }
                    }
                    else
                    {
                        if (matchingContent.MatchingContentOtherText(filePath.FullName))
                            matchFiles = matchFiles.Concat(new[] { filePath });
                    }

                }
            }
            return matchFiles;
        }
    }
}
