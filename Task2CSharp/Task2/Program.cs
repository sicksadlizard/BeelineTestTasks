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
            try
            {
                
                int wordLen = int.Parse(args[1]);
                string path = args[0];
                FileParser fileParser = new FileParser(wordLen, path);
                fileParser.Run();
                /*foreach(KeyValuePair<string, int> p in fileParser.getTopTen(path))
                {
                    Console.WriteLine("Word \"{0}\" uses [{1}] times", p.Key, p.Value);
                }*/
            }
            catch
            {

            }
            Console.ReadKey();
        }
    }
}
