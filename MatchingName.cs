using System;

namespace FindSecretDoc
{
    public class MatchingName
    {

        public MatchingName() { }

        /// <summary>
        /// Поиск соответствий в названии файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public bool FindMatchingInName(string path, string[] content)
        {
            bool result = false;
            foreach (var item in content)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    if (path.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
