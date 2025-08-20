using Microsoft.Office.Interop.Word;
using System;
using System.Text;
//using BitMiracle.Docotic.Pdf;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Texts;
using System.Runtime.CompilerServices;

namespace FindSecretDoc
{
    internal class MatchingContent
    {
        private string Content { get; set; }

        public MatchingContent() { }
        /// <summary>
        /// Поиск Соответствий в файлах
        /// </summary>
        /// <param name="content">строка для поиска соответствий</param>
        public MatchingContent(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Поиск соответствий в содержании файла docx
        /// </summary>
        /// <returns></returns>
        public bool MatchingContentForDocx(string fileName)
        {
            try
            {
                using (ProcessingMicrWordFile processingMicrWordFile = new ProcessingMicrWordFile(fileName, Content))
                {
                    
                    return processingMicrWordFile.ProcessingDocxDoc().IndexOf(Content, StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Поиск соответствий в содежаннии файлов остальных форматов
        /// </summary>
        public bool MatchingContentOtherText(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(Content))
            {
                try
                {
                    string content = System.IO.File.ReadAllText(fileName);
                    return content.IndexOf(Content, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        return false;
        }

        /// <summary>
        /// Поиск соответствий в содежаннии файлов остальных форматов PDF
        /// </summary>
        /// <param name="text">Текст в котором необходимо провести поиск соответствий</param>
        /// <returns></returns>
        public bool MatchingContentOtherTextPDF(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    return text.IndexOf(Content, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

      
    }
}

