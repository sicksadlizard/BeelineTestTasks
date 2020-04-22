using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Task2
{
    class FileParser
    {
        private int wordLength = 0;
        private string dirPath;
        private Dictionary<string, int> _topTen;
        private List<Dictionary<string, int>> _topTenList;
        private Mutex mtx = new Mutex();
        private string exp = "[a-zA-Z]{x,}";
        
        public FileParser(int l, string p)
        {
            this.wordLength = l;
            this.dirPath = p;
            exp = exp.Replace("x", wordLength.ToString());
            _topTen = new Dictionary<string, int>();
            _topTenList = new List<Dictionary<string, int>>();
        }

        private List<string> _getFilePathsList(string path)
        {
            List<string> filePaths = new List<string>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    filePaths.Add(fi.FullName);
                }
                if (filePaths.Count == 0) throw new FileNotFoundException("Directory is empty!");
            }
            catch(Exception e)
            {
                throw e;
            }
            return filePaths;
        }

        private List<Task> _createAndRunTasks(List<string> filePaths)
        {
            List<Task> _taskList = new List<Task>();
            try
            {
                foreach (string s in filePaths)
                {
                    Task tk = new Task(new Action<object>(_getTopTen), s);
                    _taskList.Add(tk);
                    tk.Start();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return _taskList;
        }

        public void Run()
        {
            try
            {
                List<string> filePaths = _getFilePathsList(dirPath);
                List<Task> _taskList = _createAndRunTasks(filePaths);                
                Task.WaitAll(_taskList.ToArray());
                _mergeResults();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private Dictionary<string, int> _getMatches(string text)
        {
            Dictionary<string, int> matches = new Dictionary<string, int>();
            try
            {
                foreach (Match m in Regex.Matches(text, exp))
                {
                    string val = m.Value.ToLower();
                    if (matches.ContainsKey(val))
                    {
                        matches[val]++;
                    }
                    else
                    {
                        matches.Add(val, 1);
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return matches;
        }

        private void _getTopTen(object fp)
        {
            
            Dictionary<string, int> topTen = new Dictionary<string, int>();
            StreamReader fs = null;

            try
            {
                string filePath = (string)fp;
                fs = new StreamReader(filePath);
                string text = fs.ReadToEnd();
                text = Regex.Replace(text, "-(\r\n)", "");
                
                topTen = _getMatches(text);
                topTen = _sortAndGetTop(topTen, 10);

                mtx.WaitOne();
                _topTenList.Add(topTen);
                mtx.ReleaseMutex();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(fs != null) fs.Close();
            }
        }

        private Dictionary<string, int> _sortAndGetTop(Dictionary<string, int> d, int count)
        {
            var sortedDict = from entry in d orderby entry.Value descending select entry;
            return sortedDict.Take(count).ToDictionary(x => x.Key, y => y.Value);
        }

        private void _mergeResults()
        {
            //MERGE
            foreach (Dictionary<string, int> d in _topTenList)
            {
                foreach (KeyValuePair<string, int> kvp in d)
                {
                    //Console.WriteLine("Word \"{0}\" matches [{1}] times.", kvp.Key, kvp.Value);
                    if (_topTen.ContainsKey(kvp.Key))
                    {
                        _topTen[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        _topTen.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            _topTen = _sortAndGetTop(_topTen, 10);
        }

        public List<string> GetTopTen()
        {
            List<String> s = new List<string>();
            foreach (KeyValuePair<string, int> kvp in _topTen)
            {
                s.Add("Word \"" + kvp.Key + "\" matches [" + kvp.Value + "] times.");
            }

            return s;
        }
    }

}
