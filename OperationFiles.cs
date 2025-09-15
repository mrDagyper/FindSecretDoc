using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using ICSharpCode.SharpZipLib.Zip;
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
                try
                {
                   // if (operationFiles.IsFileLocked(filePath))
                   // {
                        if (Settings.SearchInName)
                        {
                            var result = matching.FindMatchingInName(filePath.FullName, content);
                            if (result)
                            {
                                Settings.NameMatches.Add(filePath);
                            }
                        }

                        if (filePath.Extension == ".zip")
                        {
                            DecompressZip(filePath.FullName);
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

                    //}
                }
                catch (Exception)
                {
                    continue;
                }
               
            }

            return matchFiles;
        }

        /// <summary>
        /// Ищет соответствия в архиве ZIP
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static void DecompressZip(string zipFilePath)
        {
            var extractPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "TempFindSecretDoc";
            using (var zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry entry;
                List<FileInfo> DirectionPath  = new List<FileInfo>();

                while ((entry = zipInputStream.GetNextEntry()) != null)
                {
                    string entryPath = Path.Combine(extractPath, entry.Name);
                    if (entry.IsFile)
                    {
                        var (ResultCheckFile, file) = CheckInTheFileSuit(entry.Name);
                        if (ResultCheckFile)
                        {
                            if (!Directory.Exists(extractPath))
                                Directory.CreateDirectory(extractPath);

                            //string newPathFile = extractPath + "\\" + Path.GetRandomFileName();
                            string newPathFile = extractPath + "\\" + Path.GetRandomFileName() + file.Extension.ToString();

                            using (var fileStream = File.Create(newPathFile))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = zipInputStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, bytesRead);
                                }
                                FileInfo fileInfo = new FileInfo( newPathFile );
                                DirectionPath.Add(fileInfo);
                            }
                        }
                    }
                }

                FileInfo info = new FileInfo(zipFilePath);
                IEnumerable<FileInfo> fileInfos = new List<FileInfo>(DirectionPath);
                GetAllFiles(fileInfos, info);
                DeleteFile(extractPath);
            }
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="path">Путь удаляемого файла</param>
        private static void DeleteFile(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Проверяет подходит ли нам дайнный файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns>файл</returns>
        private static (bool, FileInfo) CheckInTheFileSuit(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Extension == ".docx" || fileInfo.Extension == ".doc" || fileInfo.Extension == ".doc"
                || fileInfo.Extension == ".txt" || fileInfo.Extension == ".pdf")
               return (true, fileInfo);
           return (false, fileInfo = null);
        }

        /// <summary>
        /// Возвращает список файлов подходящих по шаблону. Метод для работы с архивами
        /// </summary>
        /// <param name="DirectionPath">Директория</param>
        /// <param name="ArchivePath">Путь к проверяемомуАрхиву</param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetAllFiles(IEnumerable<FileInfo> DirectionPath, FileInfo ArchivePath)
        {
            var matchFiles = Enumerable.Empty<FileInfo>();

            MatchingContent matchingContent = new MatchingContent();
            ExtractText extractText = new ExtractText();
            string[] content = Settings.SearchPattern;
            OperationFiles operationFiles = new OperationFiles();

            MatchingName matching = new MatchingName();

            foreach (var filePath in DirectionPath)
            {
                try
                {
                    // if (operationFiles.IsFileLocked(filePath))
                    // {
                    if (Settings.SearchInName)
                    {
                        var result = matching.FindMatchingInName(filePath.FullName, content);
                        if (result)
                        {
                            Settings.NameMatches.Add(ArchivePath);
                        }
                    }

                    if (filePath.Extension == ".zip")
                    {
                        DecompressZip(filePath.FullName);
                    }

                    if (filePath.Extension == ".docx" || filePath.Extension == ".doc")
                    {
                        if (matchingContent.MatchingContentForDocx(filePath.FullName, content))
                            matchFiles = matchFiles.Concat(new[] { ArchivePath });
                    }
                    else if (filePath.Extension == ".pdf")
                    {
                        string text = extractText.MatchingContentForPDF(filePath.FullName);
                        if (text != null)
                        {
                            if (matchingContent.MatchingContentOtherTextPDF(text, content))
                                matchFiles = matchFiles.Concat(new[] { ArchivePath });
                        }
                    }
                    else
                    {
                        if (matchingContent.MatchingContentOtherText(filePath.FullName, content))
                            matchFiles = matchFiles.Concat(new[] { ArchivePath });
                    }

                }
                catch (Exception)
                {
                    continue;
                }

            }

            return matchFiles;
        }
    }
}
