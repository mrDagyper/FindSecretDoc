using Microsoft.Office.Interop.Word;
using System;
using System.Text;
//using BitMiracle.Docotic.Pdf;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Texts;
using System.Runtime.CompilerServices;
using System.Web;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FindSecretDoc
{
    internal class MatchingContent
    {
        public MatchingContent() { }

        /// <summary>
        /// Поиск соответствий в содержании файла docx
        /// </summary>
        /// <returns></returns>
        public bool MatchingContentForDocx(string fileName, string[] content)
        {
            try
            {
                using (ProcessingMicrWordFile processingMicrWordFile = new ProcessingMicrWordFile(fileName))
                {
                    bool result = false;
                    var text = processingMicrWordFile.ProcessingDocxDoc();
                    foreach (var item in content)
                    {
                        if (text.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            result = true;
                            break;
                        }
                    }
                    return result;
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
        public bool MatchingContentOtherText(string fileName, string[] content)
        {
            try
            {
                string contentFile = System.IO.File.ReadAllText(fileName);
                bool result = false;

                foreach (var item in content)
                {
                    if (contentFile.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Поиск соответствий в содежаннии файлов остальных форматов PDF
        /// </summary>
        /// <param name="text">Текст в котором необходимо провести поиск соответствий</param>
        /// <returns></returns>
        public bool MatchingContentOtherTextPDF(string text, string[] content)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    bool result = false; 
                    foreach (var item in content) 
                    {
                        if (text.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            result = true;
                            break;
                        }
                    }
                    return result;
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

