using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace FindSecretDoc
{
    /// <summary>
    /// Изъятие текста из фалов с расширением .doc и .docx
    /// </summary>
    internal class ProcessingMicrWordFile : IDisposable
    {
        private string FileName { get; set; }
        private string Content { get; set; }

        private readonly Application Application = new Application();
        private Document Document = new Document();

        public ProcessingMicrWordFile(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }

        /// <summary>
        /// Поиск соответствий в содержании файла .docx и .doc
        /// </summary>
        /// <returns></returns>
        public string ProcessingDocxDoc()
        {
            try
            {
                Document = Application.Documents.Open(FileName, ReadOnly: true);
                string text = Document.Content.Text;
                Document.Close();
                Application.Quit();
                return text;
            }
            catch (Exception)
            {
                Document.Close();
                Application.Quit();
                Dispose();
                return "";   
            }

        }

      
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
