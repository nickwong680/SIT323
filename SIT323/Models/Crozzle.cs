using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{
    public class Crozzle
    {
        private char[,] _CrozzleArray;
        private int _Width;
        private int _Height;

        public int Width
        {
            get { return _Width; }
        }
        public int Height
        {
            get { return _Height; }
        }

        public char this[int w, int h]
        {
            get { return _CrozzleArray[w, h]; }
        }

        public Crozzle(string fileName)
        {
            _CrozzleArray = ReadCrozzleFromFile(fileName);
            _Width = _CrozzleArray.GetUpperBound(0) + 1;
            _Height = _CrozzleArray.GetUpperBound(1) + 1;
        }

        private char[,] ReadCrozzleFromFile(string fileName)
        {
            string[] filelines = File.ReadAllLines(fileName);
            char[,] crozzle = new char[filelines.Length,filelines[0].Length];

            for (int i = 0; i < crozzle.GetLength(0) ; i++)
            {
                for (int j = 0; j < crozzle.GetLength(1); j++)
                {
                    crozzle[i,j] = Convert.ToChar(filelines[i][j]);
                }
            }
            return crozzle;
        }
    }
}
