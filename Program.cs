using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using IWshRuntimeLibrary;

namespace FindSecretDoc
{
    internal class Program
    {


        static void Main()
        {
            //*-------------------------ТЕСТ----------------------------------------
            /*string[] content = Settings.SearchPattern;
            MatchingContent matchingContent = new MatchingContent();
            matchingContent.MatchingContentForDocx(@"C:\MailUploader\ТакоеСебе.doc", content);*/
            //*=====================================================================

            Console.WriteLine("Программа для поиска файлов по названию и содержимому");
            Console.WriteLine("----------------------------------------------------");

            Console.WriteLine("Идет поиск соответствий. Ожидайте результата.");

            var driveName = DriveInfo.GetDrives();
            List<string> listFilesOut = new List<string>();

            foreach (var drive in driveName)
            {
                var listFiles = (EnumerateFiles(drive.Name));

                foreach (var item in listFiles)
                {
                    listFilesOut.Add(item);
                }

            }

            Console.SetCursorPosition(0, 3);
            Console.Write(new string(' ', SafeEnumerateFiles.LeghtConsole));
            Console.SetCursorPosition(0, 3);
            if (listFilesOut != null)
            {
                Console.WriteLine("Найдено файлов: " + listFilesOut.Count.ToString());
            }
            else
            {
                Console.WriteLine("Файлы не найдены");
            }

            DateTime dateTime = DateTime.Now;
            string writeResult = dateTime.ToString("s");
            writeResult = writeResult.Replace($":",$"-");
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var resultTextFile = baseFolder + "\\" + writeResult + ".txt";
            var resultFolder = baseFolder + "\\" + writeResult;
            Directory.CreateDirectory(resultFolder);


            System.IO.File.WriteAllText(resultTextFile, "=================== Совпадения по имени ==================================" + "\n");

            if (Settings.SearchInName)
            {
                
                Console.WriteLine("=================== Совпадения по имени ==================================");
                foreach (var SearchOnname in Settings.NameMatches)
                {
                    Console.WriteLine(SearchOnname.FullName.ToString());
                    System.IO.File.AppendAllText(resultTextFile, SearchOnname.FullName + "\n");
                }
                Console.WriteLine("========================================================================");
                System.IO.File.AppendAllText(resultTextFile, "========================================================================" + "\n");
            }
           

            Console.WriteLine("=================== Совпадения по содержанию ==================================");
            System.IO.File.AppendAllText(resultTextFile, "=================== Совпадения по содержанию ==================================" + "\n");

            string fulResultFolder = resultFolder + "\\";
            foreach (var item in listFilesOut)
            {
                FileInfo fileInfo = new FileInfo(item);
                Console.WriteLine(item.ToString());
                System.IO.File.AppendAllText(resultTextFile, item + "\n");
                CreateShortcut(item, fulResultFolder + fileInfo.Name + ".lnk");

            }
            Console.WriteLine("===============================================================================");
            Console.WriteLine("Поиск завершен!");
            System.IO.File.AppendAllText(resultTextFile, "========================================================================" + "\n");
            System.IO.File.AppendAllText(resultTextFile, "Поиск завершен!");

            Process.Start(resultTextFile);
            Process.Start("explorer.exe", resultFolder);
            Console.ReadKey();
        }

        /// <summary>
        /// Возвращает перечисляемую коллекцию имен файлов которые соответствуют шаблону в указанном каталоге, с дополнительным просмотром вложенных каталогов
        /// </summary>
        /// <param name="path">Полный или относительный путь катага в котором выполняется поиск</param>
        /// <returns>Возвращает перечисляемую коллекцию полных имен файлов</returns>
        private static List<string> EnumerateFiles(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var files = SafeEnumerateFiles.EnumerateFiles(directoryInfo, "*.*");
            List<string> matchFiles = new List<string>();
            try
            {
                foreach (var subDirPath in files)
                {
                    matchFiles.Add(subDirPath.FullName);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("DirectoryNotFoundException");
            }

            return matchFiles;
        }

        static void CreateShortcut(string targetPath, string shortcutPath)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            // Set the shortcut properties
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(targetPath); // Set working directory if needed
            shortcut.Description = "Shortcut Description"; // Set a description if desired
                                                           // Other properties you can set: IconLocation, Arguments, WindowStyle, Hotkey, etc.

            // Save the shortcut
            shortcut.Save();
        }
    }
}
