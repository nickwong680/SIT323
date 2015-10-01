using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIT323;
using SIT323.Models;

namespace SIT323Project2.Models
{
    public class CrozzleProject2
    {
        private readonly List<Word> _wordlist;
        private readonly Grid[][] _crozzleArray;

        public Grid[][] CrozzleArray
        {
            get { return _crozzleArray; }
        }

        public List<Word> Wordlist
        {
            get { return _wordlist; }
        }

        private int _width;
        private int _height;

        public CrozzleProject2(Wordlist wordlist)
        {
            //_wordlist = new List<string>(wordlist.WordList.OrderByDescending(w => w.Count()));
            _wordlist = new List<Word>();
            _width = wordlist.Width;
            _height = wordlist.Height;

            _crozzleArray = new Grid[wordlist.Height][];
            for (int i = 0; i < wordlist.Height; i++)
            {
                _crozzleArray[i] = new Grid[wordlist.Width];
                for (int j = 0; j < _crozzleArray[i].Length; j++)
                {
                    _crozzleArray[i][j] = new Grid();
                }
            }
        }

        public override string ToString()
        {
            string outString = string.Empty;
            foreach (Grid[] rows in CrozzleArray)
            {
                foreach (Grid grid in rows)
                {
                    if (grid.Character == '\0')
                    {
                        switch (grid.SpannableDirection)
                        {
                            case Direction.Horizontal:
                                outString += "-";
                                break;
                            case Direction.Vertical:
                                outString += "|";
                                break;
                            case Direction.None:
                                outString += "*";
                                break;
                            case Direction.All:
                                outString += " ";
                                break;
                        }
                    }
                    else
                    {
                        outString += grid.Character;
                    }
                }
                outString += Environment.NewLine;
            }
            return outString;
        }
    }
}