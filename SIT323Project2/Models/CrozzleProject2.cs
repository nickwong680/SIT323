using System;
using System.Collections.Generic;
using SIT323;
using SIT323.Models;

namespace SIT323Project2.Models
{
    public class CrozzleProject2
    {
        private readonly Grid[][] _crozzleArrayOfGrid;
        private readonly List<Word> _wordlist;
        private int _height;

        private int _width;

        public CrozzleProject2(Wordlist wordlist)
        {
            //_wordlist = new List<string>(wordlist.WordList.OrderByDescending(w => w.Count()));
            _wordlist = new List<Word>();
            _width = wordlist.Width;
            _height = wordlist.Height;

            _crozzleArrayOfGrid = new Grid[wordlist.Height][];
            for (var i = 0; i < wordlist.Height; i++)
            {
                _crozzleArrayOfGrid[i] = new Grid[wordlist.Width];
                for (var j = 0; j < _crozzleArrayOfGrid[i].Length; j++)
                {
                    _crozzleArrayOfGrid[i][j] = new Grid {Position = new Position {Height = i, Width = j}};
                }
            }
        }


        public Grid this[int w, int h]
        {
            get
            {
                Grid c;
                try
                {
                    c = _crozzleArrayOfGrid[w][h];
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

        public char[][] CrozzleArrayOfChar()
        {
            var outArrayOfChar = new char[_crozzleArrayOfGrid.Length][];
            for (var i = 0; i < _crozzleArrayOfGrid.Length; i++)
            {
                outArrayOfChar[i] = new char[_crozzleArrayOfGrid[i].Length];
                for (var j = 0; j < _crozzleArrayOfGrid[i].Length; j++)
                {
                    char c = (_crozzleArrayOfGrid[i][j].Character.Alphabetic == default(char)) ? default(char) : _crozzleArrayOfGrid[i][j].Character.Alphabetic;
                    outArrayOfChar[i][j] = _crozzleArrayOfGrid[i][j].Character.Alphabetic;
                }
            }
            return outArrayOfChar;
        }

        /// <summary>
        ///     Pending refactoring
        /// </summary>
        /// <returns></returns>
        public List<Span> InterectableWords()
        {
            var spans = new List<Span>();
            for (var i = 0; i < _crozzleArrayOfGrid.Length; i++)
            {
                for (var j = 0; j < _crozzleArrayOfGrid[i].Length; j++)
                {
                    var grid = this[i, j];
                    var direction = grid.SpannableDirection;
                    if (direction == Direction.None) continue;

                    var preCharacterPlaceable = new List<char>();
                    var postCharacterPlaceable = new List<char>();

                    if (!grid.IsCharacterNullOrSpaced())
                    {
                        if (grid.Character.Alphabetic == 'P' && i == 3 && j == 11)
                        {
                            var tt = 0;
                        }

                        var exit = false;
                        var back = (direction == Direction.Vertical) ? i - 1 : j - 1;
                        var next = (direction == Direction.Vertical) ? i + 1 : j + 1;

                        if (direction == Direction.Vertical)
                        {
                            while (!exit)
                            {
                                var nextGrid = this[next, j];
                                if (nextGrid == null) break;
                                switch (nextGrid.SpannableDirection)
                                {
                                    case Direction.Vertical:
                                    case Direction.All:
                                        postCharacterPlaceable.Add(nextGrid.Character.Alphabetic);
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
                                var nextGrid = this[back, j];
                                if (nextGrid == null) break;
                                switch (this[back, j].SpannableDirection)
                                {
                                    case Direction.Vertical:
                                    case Direction.All:
                                        preCharacterPlaceable.Add(this[back, j].Character.Alphabetic);
                                        break;
                                    default:
                                        exit = true;
                                        break;
                                }
                                back--;
                            }
                        }
                        else if (direction == Direction.Horizontal)
                        {
                            while (!exit)
                            {
                                var nextGrid = this[i, next];
                                if (nextGrid == null) break;
                                switch (this[i, next].SpannableDirection)
                                {
                                    case Direction.Horizontal:
                                    case Direction.All:
                                        postCharacterPlaceable.Add(this[i, next].Character.Alphabetic);
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
                                var nextGrid = this[i, back];
                                if (nextGrid == null) break;
                                switch (this[i, back].SpannableDirection)
                                {
                                    case Direction.Horizontal:
                                    case Direction.All:
                                        preCharacterPlaceable.Add(this[i, back].Character.Alphabetic);
                                        break;
                                    default:
                                        exit = true;
                                        break;
                                }
                                back--;
                            }
                        }

                        if (postCharacterPlaceable.Count + preCharacterPlaceable.Count > 0)
                        {
                            var span = new Span
                            {
                                Character = grid.Character,
                                Direction = grid.SpannableDirection,
                                Position = new Position {Height = i, Width = j},
                                PostCharacterPlaceable = postCharacterPlaceable,
                                PreCharacterPlaceable = preCharacterPlaceable
                            };
                            spans.Add(span);
                        }
                    }
                }
            }
            return spans;
        }

        public string PrintCharacter()
        {
            var outString = string.Empty;
            foreach (var rows in _crozzleArrayOfGrid)
            {
                foreach (var grid in rows)
                {
                    if (grid.Character.Alphabetic != default(char))
                    {
                        outString += grid.Character;
                    }
                }
                outString += Environment.NewLine;
            }
            return outString;
        }

        public override string ToString()
        {
            var outString = string.Empty;
            foreach (var rows in _crozzleArrayOfGrid)
            {
                foreach (var grid in rows)
                {
                    if (grid.Character.Alphabetic == default(char))
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
            outString += Environment.NewLine;
            outString += PrintDirection();

            return outString;
        }

        public string PrintDirection()
        {
            var outString = string.Empty;
            foreach (var rows in _crozzleArrayOfGrid)
            {
                foreach (var grid in rows)
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
                outString += Environment.NewLine;
            }
            return outString;
        }
    }
}