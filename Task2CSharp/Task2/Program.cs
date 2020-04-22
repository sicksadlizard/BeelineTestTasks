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
            catch(IndexOutOfRangeException e)
            {
                PrintUsage();
            }
            catch(AggregateException e)
            {
                foreach(Exception x in e.InnerExceptions)
                {
                    Console.WriteLine(x.Message);
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                PrintUsage();
            }
            Console.ReadKey();
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:\nTask2 <path> <count>\n where:");
            Console.WriteLine("<path> - path to directory, that contains files to be analized");
            Console.WriteLine("<count> - minimum word length");
        }
    }
}
