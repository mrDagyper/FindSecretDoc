using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FindSecretDoc
{
    internal class Program
    {
        

        static void Main()
        {
            Console.WriteLine("Программа для поиска файлов по названию и содержимому");
            Console.WriteLine("----------------------------------------------------");

           /*
            // Проверка существования директории
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Указанная директория не существует!");
                Console.ReadKey();
                return;
            }
           */
            Console.WriteLine("Идет поиск соответствий. Ожидайте результата.");
            /*
            // Проверка, что хотя бы один критерий поиска задан
            if (string.IsNullOrWhiteSpace(fileNameQuery) && string.IsNullOrWhiteSpace(contentQuery))
            {
                Console.WriteLine("Не задано ни одного критерия поиска!");
                return;
            }*/

            OperationFiles.searchPattern = "Secretno"; //Переделать на внешний ввод из файла
            var listFiles = EnumerateFiles("C:\\");
            Console.SetCursorPosition(0, 3);
            Console.Write(new string(' ', SafeEnumerateFiles.LeghtConsole));
            Console.SetCursorPosition(0, 3);
            if (listFiles != null)
            {
                Console.WriteLine("Найдено файлов: " + listFiles.Count.ToString());
            }
            else
            {
                Console.WriteLine("Файлы не найдены");
            }
            foreach (var item in listFiles)
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
