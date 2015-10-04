using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using SIT323;
using SIT323.Models;

namespace SIT323Project2.Models
{
    public class CrozzleProject2
    {
        private readonly Grid[][] _crozzleArrayOfGrid;
        private readonly List<Word> _wordlist;

        private Difficulty _level;

        public Difficulty Level
        {
            get { return _level; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordlist"></param>
        /// <param name="difficulty"></param>
        public CrozzleProject2(Wordlist wordlist, Difficulty level) : this(wordlist)
        {
            _level = level;
        }

        public CrozzleProject2(Wordlist wordlist)
        {
            //_wordlist = new List<string>(wordlist.WordList.OrderByDescending(w => w.Count()));
            _wordlist = new List<Word>();

            _level = wordlist.Level;

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

        public void RemoveWord(Word word)
        {
            foreach (Character c in word.CharacterList)
            {
                _crozzleArrayOfGrid[c.Position.Height][c.Position.Width] = new Grid { Position = new Position { Height = c.Position.Height, Width = c.Position.Width } };
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

        private bool DoesWordIntersectCountMeetConstraintRequirement(Grid grid)
        {
            if (Level == Difficulty.Hard || Level == Difficulty.Extreme) return true;
            if (grid.IsCharacterNullOrSpaced()) return true;
            int hor = (grid.HorizontalWord != null) ? grid.HorizontalWord.IntersectWords.Count : 0;
            int ver = (grid.VerticalWord != null) ? grid.VerticalWord.IntersectWords.Count : 0;
            int count = Math.Max (hor,ver);

            if (count >= 2)
            {
                return false;
            }
            return true;
        }

        public List<Span> FindEmptySpans()
        {
            List<Span> spans = new List<Span>();
            List<Grid> gridVer = new List<Grid>();
            List<Grid> gridHor = new List<Grid>();

            for (var i = 0; i < _crozzleArrayOfGrid.Length; i++)
            {
                for (var j = 0; j < _crozzleArrayOfGrid[i].Length; j++)
                {
                    Grid grid = this[i, j];

                    if (!grid.IsCharacterNullOrSpaced()) continue;
                    if (grid.SpannableDirection != Direction.All) continue;

                    int ver = i + 1;
                    int hor = j + 1;

                    Span spanV = new Span();
                    spanV.Position = new Position() { Height = i, Width = j };

                    do
                    {
                        var nextGrid = this[i, hor];
                        if (nextGrid == null) break;
                        if (!grid.IsCharacterNullOrSpaced()) break;

                        var direction = nextGrid.SpannableDirection;
                        if (direction == Direction.All)
                        {
                            if (!gridVer.Contains(nextGrid))
                            {
                                hor++;
                                spanV.Length++;
                                spanV.Direction = Direction.Horizontal;
                                gridVer.Add(nextGrid);
                                continue;
                            }
                        } 
                        break;
                        
                    } while (true);

                    if (spanV.Length > 1)
                    {
                        spans.Add(spanV);
                    }

                    Span spanH = new Span();
                    spanH.Position = new Position() { Height = i, Width = j };
                    do
                    {
                        var nextGrid = this[ver, j];
                        if (nextGrid == null) break;
                        if (!grid.IsCharacterNullOrSpaced()) break;

                        var direction = nextGrid.SpannableDirection;
                        if (direction == Direction.All)
                        {
                            if (!gridHor.Contains(nextGrid))
                            {
                                ver++;
                                spanH.Length++;
                                spanH.Direction = Direction.Vertical;
                                gridHor.Add(nextGrid);
                                continue;
                            }
                        }
                        break;
                    } while (true);

                    if (spanH.Length > 1)
                    {
                        spans.Add(spanH);
                    }
                }
            }
            return spans;

        } 

        /// <summary>
        ///     Pending refactoring
        /// </summary>
        /// <returns></returns>
        public List<SpanWithCharater> FindInterectableWords()
        {
            var spans = new List<SpanWithCharater>();
            for (var i = 0; i < _crozzleArrayOfGrid.Length; i++)
            {
                for (var j = 0; j < _crozzleArrayOfGrid[i].Length; j++)
                {
                    var grid = this[i, j];
                    if (!DoesWordIntersectCountMeetConstraintRequirement(grid)) continue;

                    var direction = grid.SpannableDirection;
                    if (direction == Direction.None) continue;

                    var preCharacterPlaceable = new List<Character>();
                    var postCharacterPlaceable = new List<Character>();

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
                                if (!DoesWordIntersectCountMeetConstraintRequirement(grid)) continue;

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
                                var nextGrid = this[back, j];
                                if (nextGrid == null) break;
                                if (!DoesWordIntersectCountMeetConstraintRequirement(grid)) continue;

                                switch (this[back, j].SpannableDirection)
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
                        else if (direction == Direction.Horizontal)
                        {
                            while (!exit)
                            {
                                var nextGrid = this[i, next];
                                if (nextGrid == null) break;
                                if (!DoesWordIntersectCountMeetConstraintRequirement(grid)) continue;

                                switch (this[i, next].SpannableDirection)
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
                                var nextGrid = this[i, back];
                                if (nextGrid == null) break;
                                if (!DoesWordIntersectCountMeetConstraintRequirement(grid)) continue;

                                switch (this[i, back].SpannableDirection)
                                {
                                    case Direction.Horizontal:
                                    case Direction.All:
                                        preCharacterPlaceable.Add(this[i, back].Character);
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
                            var span = new SpanWithCharater
                            {
                                Character = grid.Character,
                                Direction = grid.SpannableDirection,
                                Position = new Position {Height = i, Width = j},
                                PostCharacterPlaceable = postCharacterPlaceable,
                                PreCharacterPlaceable = preCharacterPlaceable,
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