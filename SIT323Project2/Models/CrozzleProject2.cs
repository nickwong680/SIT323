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

        public Grid this[int w, int h]
        {
            get
            {
                Grid c;
                try
                {
                    c = _crozzleArray[w][h];
                }
                catch (IndexOutOfRangeException e)
                {
                    c = null;
                }
                return c;
            }
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
        /// <summary>
        /// Pending refactoring
        /// </summary>
        /// <returns></returns>
        public List<Span> InterectableWords()
        {
            List<Span> spans = new List<Span>();
            for (int i = 0; i < _crozzleArray.Length; i++)
            {
                for (int j = 0; j < _crozzleArray[i].Length; j++)
                {
                    Grid grid = this[i,j];
                    Direction direction = grid.SpannableDirection;
                    if (direction == Direction.None) continue;

                    List<char> preCharacterPlaceable = new List<char>();
                    List<char> postCharacterPlaceable = new List<char>();

                    if (grid.Character != '\0')
                    {
                        bool exit = false;
                        int back = i - 1;
                        int next = i + 1;

                        if (direction == Direction.Vertical)
                        {
                            while (!exit)
                            {
                                Grid nextGrid = this[next,j];
                                if (nextGrid == null) break;
                                switch (nextGrid.SpannableDirection)
                                {
                                    case Direction.Vertical:
                                    case Direction.All:
                                        postCharacterPlaceable.Add(nextGrid.Character);
                                        break;
                                    default:
                                        exit = true;
                                        break;
                                }
                                next++;
                            }
                            exit = false;
                            while (!exit)
                            {
                                Grid nextGrid = this[back,j];
                                if (nextGrid == null) break;
                                switch (this[back,j].SpannableDirection)
                                {
                                    case Direction.Vertical:
                                    case Direction.All:
                                        preCharacterPlaceable.Add(this[back, j].Character);
                                        break;
                                    default:
                                        exit = true;
                                        break;
                                }
                                back--;
                            }
                        }
                        else
                        {
                            while (!exit)
                            {
                                Grid nextGrid = this[i,next];
                                if (nextGrid == null) break;
                                switch (this[i,next].SpannableDirection)
                                {
                                    case Direction.Horizontal:
                                    case Direction.All:
                                        postCharacterPlaceable.Add(this[i, next].Character);
                                        break;
                                    default:
                                        exit = true;
                                        break;
                                }
                                next++;
                            }
                            exit = false;
                            while (!exit)
                            {
                                Grid nextGrid = this[i,back];
                                if (nextGrid == null) break;
                                switch (this[i,back].SpannableDirection)
                                {
                                    case Direction.Vertical:
                                    case Direction.All:
                                        preCharacterPlaceable.Add(this[i,back].Character);
                                        break;
                                    default:
                                        exit = true;
                                        break;
                                }
                                back--;
                            }
                        }
                        Span span = new Span()
                        {
                            Character = grid.Character,
                            Direction = grid.SpannableDirection,
                            Position = new Position() { Height = i, Width = j, },
                            PostCharacterPlaceable = postCharacterPlaceable,
                            PreCharacterPlaceable = preCharacterPlaceable,
                        };
                        spans.Add(span);
                    }
                }
            }
            return spans;
        }
        public override string ToString()
        {
            string outString = string.Empty;
            foreach (Grid[] rows in _crozzleArray)
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