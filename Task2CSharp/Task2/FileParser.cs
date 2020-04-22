using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task2
{
    class FileParser
    {
        private int wordLength = 0;
        private string dirPath;
        string exp;

        public FileParser(int l, string p)
        {
            this.wordLength = l;
            this.dirPath = p;
            exp = "[a-zA-Z]{x,}";
            exp = exp.Replace("x", wordLength.ToString());
        }

        public bool Run()
        {

            return true;
        }

        public Dictionary<string, int> getTopTen(string filePath)
        {
            Dictionary<string, int> topTen = new Dictionary<string, int>();
            StreamReader fs = null;

            try
            {
                fs = new StreamReader(filePath);
                string text = fs.ReadToEnd();
                text = Regex.Replace(text, "-(\r\n)", "");
                foreach(Match m in Regex.Matches(text, exp))
                {
                    Console.WriteLine(m.Value);
                    string val = m.Value.ToLower();
                    if(topTen.ContainsKey(val))
                    {
                        topTen[val]++;
                    }
                    else
                    {
                        topTen.Add(val, 1);
                    }
                }
                var sortedDict = from entry in topTen orderby entry.Value descending select entry;
                topTen = sortedDict.Take(10).ToDictionary(x => x.Key, y => y.Value);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(fs != null) fs.Close();
            }

            return topTen;
        }
    }
}
