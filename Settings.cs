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
                List<string> strArr = new List<string>();
                if (arrSearchPattern != null)
                {
                    var strSplit = arrSearchPattern.Split(',');
                    foreach (var item in strSplit)
                    {
                        strArr.Add(" " + item);
                        strArr.Add(item + " ");
                    }
                    return strSplit.ToArray();

                }
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

        public static bool ExitTextFile
        {
            get
            {
                string strExitTextFile = ConfigurationManager.AppSettings["exitTextFile"];
                bool exitTextFile = bool.TryParse(strExitTextFile, out bool ComplParse);
                if(ComplParse)
                    return exitTextFile;
                return false;
            }
        }

    }
}
