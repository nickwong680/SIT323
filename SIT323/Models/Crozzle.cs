using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIT323.Models
{
    /// <summary>
    /// Reads Test # - crozzle.txt file
    /// Uses Jagged Array for storage of characters
    /// Implements Ilogger for logging errors
    /// </summary>
    public class Crozzle : ILogger
    {
        private readonly char[][] _crozzleArray;
        private Wordlist _wordlist;

        /// <summary>
        /// Constructor that takes the file name of the crozzle and the Wordlist object previously constucted
        /// It init the array and call appropriate validators
        /// It logs any error alone the way.
        /// </summary>
        /// <param name="fileName">file name in string</param>
        /// <param name="wordlist">wordlist object</param>
        public Crozzle(string fileName, Wordlist wordlist)
        {
            _wordlist = wordlist;
            _crozzleArray = ReadCrozzleFromFile(fileName);

            Init();

//            Width = _crozzleArray.Length;
//            Height = _crozzleArray[0].Length;
//
//            LogList = new List<LogMessage>();
//            LogList.AddRange(new CrozzleValidator(this)
//                .AreCellsSizeCorrectAccordingToHeader(wordlist.Width, wordlist.Height)
//                .AreCellsSizeCorrectAccordingToRequirement(wordlist.Width, wordlist.Height)
//                .AreCellsValidAlphabet()
//                .LogList
//                );
        }

        public Crozzle(char[][] generatedCrozzle, Wordlist wordlist)
        {
            _wordlist = wordlist;
            _crozzleArray = generatedCrozzle;

            Init();
        }

        private void Init()
        {
            Width = _crozzleArray.Length;
            Height = _crozzleArray[0].Length;

            LogList = new List<LogMessage>();
            LogList.AddRange(new CrozzleValidator(this)
                .AreCellsSizeCorrectAccordingToHeader(_wordlist.Height, _wordlist.Width)
                .AreCellsSizeCorrectAccordingToRequirement(_wordlist.Height, _wordlist.Width)
                .AreCellsValidAlphabet()
                .LogList
                );
        }
        /// <summary>
        /// Array width and height
        /// </summary>
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// @See public char this[int w, int h]
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public char[] this[int i]
        {
            get { return _crozzleArray[i]; }
        }

        /// <summary>
        ///     Indexer for Jagged array.
        ///     It looks like a multiple dimesmontion array indexer but it's not.
        /// </summary>
        /// <param name="w">the width</param>
        /// <param name="h">the height</param>
        /// <returns></returns>
        public char this[int w, int h]
        {
            get
            {
                char c;
                try
                {
                    c = _crozzleArray[w][h];
                }
                catch (IndexOutOfRangeException e)
                {
                    c = ' ';
                }
                return c;
            }
        }
        /// <summary>
        /// List of Logessage as per required by ILogger
        /// </summary>
        public List<LogMessage> LogList { get; set; }

        /// <summary>
        /// Return all log message in one concatenated string as per required by ILogger
        /// </summary>
        /// <returns>string of all message</returns>
        public string LogListInString()
        {
            return string.Join(Environment.NewLine, LogList.Select(l => l.ToString()));
        }
        /// <summary>
        /// Reads Crozzle and returns content back in Jagged array
        /// </summary>
        /// <param name="fileName">name of file</param>
        /// <returns>Jagged array of Crozzle</returns>
        private char[][] ReadCrozzleFromFile(string fileName)
        {
            var filelines = File.ReadAllLines(fileName);
            var crozzle = new char[filelines.Length][];

            for (var i = 0; i < filelines.Length; i++)
            {
                crozzle[i] = new char[filelines[i].Length];
                for (var j = 0; j < filelines[i].Length; j++)
                {
                    crozzle[i][j] = filelines[i][j];
                }
            }
            return crozzle;
        }

        public string PrintCharacter()
        {
            var outString = string.Empty;
            foreach (var rows in _crozzleArray)
            {
                foreach (var grid in rows)
                {
                    if (grid != default(char))
                    {
                        outString += (grid == default(char)) ? " " : grid.ToString();
                    }
                    else
                    {
                        outString += " ";
                    }
                }
                outString += Environment.NewLine;
            }
            return outString;
        }
    }
}