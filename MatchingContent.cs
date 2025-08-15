using Microsoft.Office.Interop.Word;
using System;

namespace FindSecretDoc
{
    internal class MatchingContent : IDisposable
    {
        private string FileName { get; set; }
        private string Content { get; set; }

        private Application Application = new Application();
        private Document Document = new Document();

        private MatchingContent() { }
        /// <summary>
        /// Поиск Соответствий в файлах
        /// </summary>
        /// <param name="fileName">путь к файлу</param>
        /// <param name="content">строка для поиска соответствий</param>
        public MatchingContent(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }

        /// <summary>
        /// Поиск соответствий в содержании файла docx
        /// </summary>
        /// <returns></returns>
        public bool MatchingContentForDocx(/*string fileName, string searchPattern*/)
        {
            try
            {
                Document = Application.Documents.Open(FileName, ReadOnly: true);
                Boolean result = Document.Content.Text.IndexOf(Content, StringComparison.CurrentCultureIgnoreCase) >= 0;
                Document.Close();
                Application.Quit();

                return result;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public void Dispose()
        {
            Application.Quit();
            GC.SuppressFinalize(this);
        }
    }
}

