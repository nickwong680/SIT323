using System;
using System.Collections;
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
        readonly int MinWordCount = 10;
        readonly int MaxWordCount = 1000;
        readonly int MinCrozzleWeightCount = 4;
        readonly int MaxCrozzleHeightCount = 400;

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

            List<LogMessage> LogList = new List<LogMessage>();

            LogList.AddRange(new IntValidator(file[0], "field 0").IsInRange(MinWordCount, MaxWordCount).LogList);
            LogList.AddRange(new IntValidator(file[1], "field 0").IsInRange(MinCrozzleWeightCount, MaxCrozzleHeightCount).LogList);
            LogList.AddRange(new IntValidator(file[2], "field 0").IsInRange(MinCrozzleWeightCount, MaxCrozzleHeightCount).LogList);

            var stringvalidtor = new StringValidtor(":-", "word");
            var stringvalidtor2 = new StringValidtor("", "word");



            int.TryParse(file[0], out _WordsCount);
            int.TryParse(file[1], out _Width);
            int.TryParse(file[2], out _Height);

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