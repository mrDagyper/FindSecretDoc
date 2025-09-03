using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            OperationFiles operationFiles = new OperationFiles();

            MatchingName matching = new MatchingName();

            foreach (var filePath in DirectionPath)
            {
                   
                if (operationFiles.IsFileLocked(filePath))
                {
                        if (Settings.SearchInName)
                        {
                            var result = matching.FindMatchingInName(filePath.FullName, content);
                            if (result)
                            {
                                Settings.NameMatches.Add(filePath);
                            }
                        }

                     /* if (filePath.Extension == ".zip")
                        {
                            ExtractZipArchive(filePath.FullName);
                        }*/

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

        /// <summary>
        /// Распаковывает архив во временную папку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static void ExtractZipArchive(string path)
        {
            //string zipPath = @".\result.zip";

           // Console.WriteLine("Provide path where to extract the zip file:");
           // string extractPath = Console.ReadLine();

            // Normalizes the path.
           // extractPath = Path.GetFullPath(extractPath);
            string extractPath = path;

            // Ensures that the last character on the extraction path
            // is the directory separator char.
            // Without this, a malicious zip file could try to traverse outside of the expected
            // extraction path.
            if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                extractPath += Path.DirectorySeparatorChar;

            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                //IEnumerable<FileInfo> files = null;
                List<FileInfo> files = new List<FileInfo>();
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    try
                    {
                        /* if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)) //<--- это нахер отсюда
                         {
                             // Gets the full path to ensure that relative segments are removed.
                             string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                             // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                             // are case-insensitive.
                             if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                                 entry.ExtractToFile(destinationPath);
                         }*/
                        files.Add(new FileInfo(entry.Name));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                   
                }

                GetAllFiles(files);


            }
        }

        /// <summary>
        /// Проверка на доступность файла
        /// </summary>
        /// <param name="file">файл</param>
        /// <returns></returns>
        protected bool IsFileLocked(FileInfo file)
        {
            try
            {
                if (file.Exists)
                {
                    using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            }

            //file is not locked
            return true;
        }
    }
}
