using System;
using System.Collections.Generic;
using System.IO;

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

            if (Settings.SearchInName)
            {
                Console.WriteLine("=================== Совпадения по имени ==================================");
                foreach (var SearchOnname in Settings.NameMatches)
                {
                    Console.WriteLine(SearchOnname.FullName.ToString());
                }
                Console.WriteLine("========================================================================");
            }
           

            Console.WriteLine("=================== Совпадения по содержанию ==================================");
            foreach (var item in listFilesOut)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("===============================================================================");
            Console.WriteLine("Поиск завершен!");
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
    }
}
