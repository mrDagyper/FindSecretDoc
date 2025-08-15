using System;

namespace FindSecretDoc
{
    public class MatchingName
    {
        private string Content { get; set; }

        private MatchingName() { }
        public MatchingName(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Поиск соответствий в названии файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public bool FindMatchingInName(string path/*, string searchPattern*/)
        {
            if (!string.IsNullOrWhiteSpace(Content))
                return path.IndexOf(Content, StringComparison.OrdinalIgnoreCase) >= 0;

            return false;
        }
    }
}
