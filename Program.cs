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
            //MatchingContent matchingContent = new MatchingContent();
            //matchingContent.MatchingContentForPDF(@"C:\MailUploader\Секретно.pdf");
            //*=====================================================================

            Console.WriteLine("Программа для поиска файлов по названию и содержимому");
            Console.WriteLine("----------------------------------------------------");

            Console.WriteLine("Идет поиск соответствий. Ожидайте результата.");

            var driveName = DriveInfo.GetDrives();
            List<string> listFilesOut = new List<string>();
            OperationFiles.searchPattern = "Secretno"; //Переделать на внешний ввод из файла

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
            foreach (var item in listFilesOut)
            {
                Console.WriteLine(item.ToString());
            }
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
