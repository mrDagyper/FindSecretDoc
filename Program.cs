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

            StartProgramTextConsole();
            List<string> listFilesOut = FileIteration();
            ResultIterationFiles(listFilesOut);

            DateTime dateTime = DateTime.Now;
            string writeResult = dateTime.ToString("s");
            writeResult = writeResult.Replace($":", $"-");
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var resultTextFile = baseFolder + "\\" + writeResult + ".txt";
            var resultFolder = baseFolder + "\\" + writeResult;
            Directory.CreateDirectory(resultFolder);

            Program program = new Program();

            if (Settings.ExitTextFile)
                System.IO.File.WriteAllText(resultTextFile, program.NameMatchTextConsole() + "\n");

            if (Settings.SearchInName)
            {
                if (Settings.ExitTextFile)
                    Console.WriteLine(program.NameMatchTextConsole());

                foreach (var SearchOnname in Settings.NameMatches)
                {
                    Console.WriteLine(SearchOnname.FullName.ToString());

                    if (Settings.ExitTextFile)
                        System.IO.File.AppendAllText(resultTextFile, SearchOnname.FullName + "\n");
                }
                Console.WriteLine(program.IntermediateTextConsole());

                if (Settings.ExitTextFile)
                    System.IO.File.AppendAllText(resultTextFile, program.IntermediateTextConsole() + "\n");
            }


            Console.WriteLine(program.ContentMatchTextConsole());

            if (Settings.ExitTextFile)
                System.IO.File.AppendAllText(resultTextFile, program.ContentMatchTextConsole() + "\n");

            string fulResultFolder = resultFolder + "\\";
            foreach (var item in listFilesOut)
            {
                FileInfo fileInfo = new FileInfo(item);
                Console.WriteLine(item.ToString());

                if (Settings.ExitTextFile)
                    System.IO.File.AppendAllText(resultTextFile, item + "\n");

                CreateShortcut(item, fulResultFolder + fileInfo.Name + ".lnk");

            }
            Console.WriteLine(program.IntermediateTextConsole());
            Console.WriteLine("Поиск завершен!");
            if (Settings.ExitTextFile)
            {
                System.IO.File.AppendAllText(resultTextFile, program.IntermediateTextConsole() + "\n");
                System.IO.File.AppendAllText(resultTextFile, "Поиск завершен!");
            }


            if (Settings.ExitTextFile)
                Process.Start(resultTextFile);

            Process.Start("explorer.exe", resultFolder);
            Console.ReadKey();
        }

        /// <summary>
        /// Результат итерации фйлов
        /// </summary>
        /// <param name="listFilesOut">коллекция найденых файлов</param>
        private static void ResultIterationFiles(List<string> listFilesOut)
        {
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
        }

        /// <summary>
        /// Итерация файлов
        /// </summary>
        /// <returns>Коллекция путец к файлам найденых соответствий</returns>
        private static List<string> FileIteration()
        {
            var driveName = DriveInfo.GetDrives();
            List<string> listFilesOut = new List<string>();

           // foreach (var drive in driveName)
            //{
                //var listFiles = (EnumerateFiles(drive.Name)); // закоментить для теста
                var listFiles = (EnumerateFiles(PathForTest())); //закоментить для боевого режима

                foreach (var item in listFiles)
                {
                    listFilesOut.Add(item);
                }

           // }

            return listFilesOut;
        }

        private static string PathForTest()
            => "C:\\MailUploader";

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

        /// <summary>
        /// Создание ярлыка
        /// </summary>
        /// <param name="targetPath">Путь для кого ярлык</param>
        /// <param name="shortcutPath">путь где будет распологаться ярлык</param>
        static void CreateShortcut(string targetPath, string shortcutPath)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(targetPath);
            shortcut.Description = "Shortcut Description";
            shortcut.Save();
        }

        /// <summary>
        /// Текстовое начало отображения
        /// </summary>
        /// <returns></returns>
        private string NameMatchTextConsole()
            => "=================== Совпадения по имени ==================================";

        /// <summary>
        /// Промежуточный текст (==========)
        /// </summary>
        /// <returns></returns>
        private string IntermediateTextConsole()
            => "========================================================================";

        /// <summary>
        /// Текстовое окончание отображения
        /// </summary>
        /// <returns></returns>
        private string ContentMatchTextConsole()
            => "=================== Совпадения по содержанию ==================================";

        /// <summary>
        /// Текстовый старт программы
        /// </summary>
        private static void StartProgramTextConsole()
        {
            Console.WriteLine("Программа для поиска файлов по названию и содержимому");
            Console.WriteLine("----------------------------------------------------");

            Console.WriteLine("Идет поиск соответствий. Ожидайте результата.");
        }
    }
}
