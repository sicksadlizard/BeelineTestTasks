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
                foreach(string s in fileParser.GetTopTen())
                {
                    Console.WriteLine(s);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
