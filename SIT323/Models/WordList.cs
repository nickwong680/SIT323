using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }
    /// <summary>
    /// For Validation, Wordlist is coupled with Validator class and it's subclass IntValidator, StringValidtor and WordListValidator.
    /// </summary>
    public class Wordlist : ILogger
    {
        public readonly int MinWordCount = 10;
        public readonly int MaxWordCount = 1000;
        public readonly int MinCrozzleWeightCount = 4;
        public readonly int MaxCrozzleHeightCount = 400;

        private Difficulty _Level;
        private int _WordsCount;
        private int _Width;
        private int _Height;
        private List<string> _WordList;
        public List<LogMessage> LogList { get; set; }

        public string LogListInString()
        {
            return string.Join(Environment.NewLine, LogList.Select(l => l.ToString()));
        }
        public List<string> WordList
        {
            get { return _WordList; }
        }
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

            LogList = new List<LogMessage>();

            LogList.AddRange(new IntValidator(file[0], "field 0").IsInRange(MinWordCount, MaxWordCount).LogList);
            LogList.AddRange(new IntValidator(file[1], "field 1").IsInRange(MinCrozzleWeightCount, MaxCrozzleHeightCount).LogList);
            LogList.AddRange(new IntValidator(file[2], "field 2").IsInRange(MinCrozzleWeightCount, MaxCrozzleHeightCount).LogList);
            int.TryParse(file[1], out _Width);
            int.TryParse(file[2], out _Height);

            _WordList = file.ToList();
            _WordList.RemoveRange(0,3);
            foreach (var word in _WordList)
            {
                LogList.AddRange(new StringValidtor(word, "word").LogList);
            }
            LogList.AddRange(new WordListValidator<string>(_WordList, "wordlist").IsInRange(MinWordCount, MaxWordCount).LogList);

            switch (file[3].ToUpper())
            {
                case "EASY":
                    _Level = Difficulty.Easy;
                    break;
                case "MEDIUM":
                    _Level = Difficulty.Medium;
                    break;
                case "HARD":
                    _Level = Difficulty.Hard;
                    break;
                case "EXTREME":
                    _Level = Difficulty.Extreme;
                    break;
            }
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