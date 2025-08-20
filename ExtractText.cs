using Spire.Pdf.Texts;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindSecretDoc
{
    public class ExtractText
    {
        public ExtractText() { }

        /// <summary>
        /// Извлечение текста из PDF документа (ограничение бесплатной версии 10 страниц PDF файла)
        /// </summary>
        /// <returns></returns>
        public string MatchingContentForPDF(string path)
        {
            using (PdfDocument document = new PdfDocument())
            {
                try
                {
                    document.LoadFromFile(path);
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (PdfPageBase page in document.Pages)
                    {
                        PdfTextExtractor pdfTextExtractor = new PdfTextExtractor(page);
                        PdfTextExtractOptions options = new PdfTextExtractOptions
                        {
                            IsExtractAllText = true
                        };
                        string text = pdfTextExtractor.ExtractText(options);
                        stringBuilder.AppendLine(text);
                    }
                    return stringBuilder.ToString();
                }
                catch (Exception)
                {

                    return null;
                }
              
            }
        }
    }
}
