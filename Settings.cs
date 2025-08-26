using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindSecretDoc
{
    static class Settings
    {
        public static List<FileInfo> NameMatches = new List<FileInfo>();

        /// <summary>
        /// Слова для поиска совпадений
        /// </summary>
        public static string[] SearchPattern
        {
            get 
            {
                var arrSearchPattern = ConfigurationManager.AppSettings["searchPattern"];
                if (arrSearchPattern != null)
                    return arrSearchPattern.Split(',');
                return null;
            }
        }

        /// <summary>
        /// Искать ли совпадения в имени
        /// </summary>
        public static bool SearchInName
        {
            get
            {
                string strSearchInName = ConfigurationManager.AppSettings["SearchInName"];
                bool searchInName = bool.TryParse(strSearchInName, out bool ComplParse);
                if (ComplParse)
                    return searchInName;
                return false;
            }
        }

       /* /// <summary>
        /// Совпадения в наименовании
        /// </summary>
        public static List<bool> NameMatches { get; set; }*/

    }
}
