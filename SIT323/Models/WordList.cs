using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIT323
{
    /// <summary>
    ///     enum of Difficulty in Easy Medium Hard or Extreme
    /// </summary>
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }

    /// <summary>
    ///     Reads Test # - wordlist.csv file
    ///     For Validation, Wordlist is coupled with Validator class and it's subclass IntValidator, StringValidtor and
    ///     WordListValidator.
    /// </summary>
    public class Wordlist : ILogger
    {
        /// <summary>
        ///     Height parsed from file
        /// </summary>
        private readonly int _height;

        /// <summary>
        ///     Width parsed from file
        /// </summary>
        private readonly int _width;

        /// <summary>
        ///     Variables for requirements defined in Assignment 1.pdf
        /// </summary>
        public readonly int MaxCrozzleHeightCount = 400;
        public readonly int MaxWordCount = 1000;
        public readonly int MinCrozzleWeightCount = 4;
        public readonly int MinWordCount = 10;

        /// <summary>
        ///     Constructor for wordlist takes filename of wordlist file
        ///     Validate header fields and log any error messages
        ///     Words are added to a List of WordList
        ///     Difficulty level is parsed
        /// </summary>
        /// <param name="fileName">filename in string</param>
        public Wordlist(string fileName)
        {
            string[] file = ReadWordListFromFile(fileName);

            LogList = new List<LogMessage>();

            LogList.AddRange(new IntValidator(file[0], "field 0").IsInRange(MinWordCount, MaxWordCount).LogList);
            LogList.AddRange(
                new IntValidator(file[1], "field 1").IsInRange(MinCrozzleWeightCount, MaxCrozzleHeightCount).LogList);
            LogList.AddRange(
                new IntValidator(file[2], "field 2").IsInRange(MinCrozzleWeightCount, MaxCrozzleHeightCount).LogList);
            int.TryParse(file[1], out _height);
            int.TryParse(file[2], out _width);

            WordList = file.ToList();
            WordList.RemoveRange(0, 4);
            foreach (var word in WordList)
            {
                LogList.AddRange(new StringValidtor(word, "word").LogList);
            }
            LogList.AddRange(
                new WordListValidator<string>(WordList, "wordlist").IsInRange(MinWordCount, MaxWordCount).LogList);

            switch (file[3].ToUpper())
            {
                case "EASY":
                    Level = Difficulty.Easy;
                    break;
                case "MEDIUM":
                    Level = Difficulty.Medium;
                    break;
                case "HARD":
                    Level = Difficulty.Hard;
                    break;
                case "EXTREME":
                    Level = Difficulty.Extreme;
                    break;
            }
        }

        /// <summary>
        ///     public get porperty for List of words
        /// </summary>
        public List<string> WordList { get; private set; }

        /// <summary>
        ///     public get porperty for Difficulty of level
        /// </summary>
        public Difficulty Level { get; private set; }

        /// <summary>
        ///     public get porperty number of words
        /// </summary>
        public int WordsCount { get; private set; }

        /// <summary>
        ///     getter porperty for width
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        ///     getter porperty for height
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        ///     List of Logessage as per required by ILogger
        /// </summary>
        public List<LogMessage> LogList { get; set; }

        /// <summary>
        ///     Return all log message in one concatenated string as per required by ILogger
        /// </summary>
        /// <returns>string of all message</returns>
        public string LogListInString()
        {
            return string.Join(Environment.NewLine, LogList.Select(l => l.ToString()));
        }

        /// <summary>
        ///     Reads a wordlist and return it in a array
        ///     Assumes all strings are on a single
        ///     strings are then splits into substrings using ',' spectator
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