using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }

    public class Wordlist
    {
        private Difficulty _Level;
        private int _WordsCount;
        private int _Width;
        private int _Height;
        private List<string> _WordList;

        public Difficulty Level
        {
            get { return _Level; }
        }

        public int WordsCount
        {
            get { return _WordsCount; }
        }

        public int Width
        {
            get { return _Width; }
        }

        public int Height
        {
            get { return _Height; }
        }

        public Wordlist(string fileName)
        {
            string[] file = ReadWordListFromFile(fileName);
            _WordsCount = Convert.ToInt32(file[0]);
            _Width = Convert.ToInt32(file[1]);
            _Height = Convert.ToInt32(file[2]);
            _WordList = file.ToList();
            _WordList.RemoveRange(0,3);

        }

        /// <summary>
        /// Reads a wordlist and return it in a array 
        /// Assumes all strings are on a single
        /// strings are then splits into substrings using ',' spectator
        /// 
        /// </summary>
        /// <param name="fileName">name of the wordlist</param>
        /// <returns>return a array of string</returns>
        private string[] ReadWordListFromFile(string fileName)
        {
            string[] fileStrings = File.ReadAllLines(fileName);
            return fileStrings[0].Split(',');
        }
    }
}