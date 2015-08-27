using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{
    public class Crozzle : ILogger
    {
        private char[][] _CrozzleArray;
        private int _Width;
        private int _Height;
        public List<LogMessage> LogList { get; set; }

        private Wordlist _Wordlist;

        public int Width
        {
            get { return _Width; }
        }
        public int Height
        {
            get { return _Height; }
        }

        public char[] this[int i]
        {
            get { return _CrozzleArray[i]; }
        }

        /// <summary>
        /// Indexer for Jagged array.
        /// It looks like a multiple dimesmontion array but it's not.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public char this[int w,int h]
        {
            get { return _CrozzleArray[w][h]; }
        }

        public Crozzle(string fileName, Wordlist wordlist)
        {
            _Wordlist = wordlist;
            _CrozzleArray = ReadCrozzleFromFile(fileName);
            _Width = _CrozzleArray.Length;
            _Height = _CrozzleArray[0].Length;

            LogList = new List<LogMessage>();
            LogList.AddRange(new CrozzleValidator(this)
                .AreCellsSizeCorrect(wordlist.Width, wordlist.Height)
                .AreCellsValidAlphabet()
                .LogList
                );
        }

 
        private char[][] ReadCrozzleFromFile(string fileName)
        {
            string[] filelines = File.ReadAllLines(fileName);
            char[][] crozzle = new char[filelines.Length][];

            for (int i = 0; i < filelines.Length; i++)
            {
                crozzle[i] = new char[filelines[i].Length];
                for (int j = 0; j < filelines[i].Length; j++)
                {
                    crozzle[i][j] = filelines[i][j];
                }
            }
            return crozzle;
        }

    }
}
