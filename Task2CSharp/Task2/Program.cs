using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            int wordLen = 5;
            string path = "text.txt";
            FileParser fileParser = new FileParser(wordLen, path);
            fileParser.getTopTen(path);
        }
    }
}
