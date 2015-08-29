using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using SIT323.Models;

namespace SIT323
{

    public abstract class Validator: ILogger
    {
        protected string location;
        public List<LogMessage> LogList { get; set; }
        protected bool IsEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = "is empty"
                });
                return true;
            }
            return false;
        }
    }

    public class StringValidtor : Validator
    {
        private string value;
        public StringValidtor(string s, string location)
        {
            LogList = new List<LogMessage>();
            this.location = location;

            IsEmpty(s);
            IsAlphabetic(s);
            value = s;
        }

        private bool IsAlphabetic(string s)
        {
            if (s.Any(x => !char.IsLetter(x)))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("word value ({0}) is not an alphabetic", s)
                });
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Validator is a Builder 
    /// </summary>
    public class IntValidator : Validator
    {
        protected int value = -1;

        public IntValidator(string s, string location)
        {
            LogList = new List<LogMessage>();
            this.location = location;

            if (!IsEmpty(s)) if(IsInt(s)) int.TryParse(s, out value);
        }

        private bool IsInt(string s)
        {
            if (s.Any(x => !char.IsDigit(x)))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("value ({0}) is not an integer", s)
                });
                return false;
            }
            return true;
        }

        public IntValidator IsInRange(int min, int max)
        {
            if (value == -1) return this;
            if (value < min || value > max)
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("size value ({0}) is not in range", value)
                });
            return this;
        }
    }

    public class WordListValidator<T> : Validator
    {
        private List<T> list;
        public WordListValidator(List<T> l, string location)
        {
            list = l;
            LogList = new List<LogMessage>();
            this.location = location;

            IsEmpty(string.Join(",", list.ToArray()));
        }

        public WordListValidator<T> IsInRange(int min, int max)
        {
            if (list.Count == 0) return this;
            if (list.Count < min || list.Count > max)
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("{0} contains ({1}) words. Expecting {2} to {3} words", location, list.Count, min, max)
                });
            return this;
        }
    }


    public class CrozzleValidator : Validator
    {
        private Crozzle crozzle;
        public CrozzleValidator(Crozzle c)
        {
            LogList = new List<LogMessage>();
            crozzle = c;
        }

        private void AreCellsSizeCorrect(int row, int columns, string rowMsg, string columnsMsg)
        {
            if (crozzle.Width != row)
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = "Row",
                    TextMessage = string.Format(rowMsg, crozzle.Width, row)
                });
            }
            for (int i = 0; i < crozzle.Width; i++)
            {
                if (crozzle[i].Length != columns)
                {
                    LogList.Add(new LogMessage()
                    {
                        Level = Level.Error,
                        Location = "Columns",
                        TextMessage = string.Format(columnsMsg, i, crozzle[i].Length, columns)
                    });
                }
            }
        }
        public CrozzleValidator AreCellsSizeCorrectAccordingToRequirement(int row, int columns)
        {
            AreCellsSizeCorrect(row, columns, "file contains ({0}) rows, instead of {1} rows as per requirement",
                "row {0} contains ({1}) columns, instead of {2} columns as per requirement");
            return this;
        }
        public CrozzleValidator AreCellsSizeCorrectAccordingToHeader(int row, int columns)
        {
            AreCellsSizeCorrect(row, columns, "file contains ({0}) rows, instead of {1} rows as per header",
    "row {0} contains ({1}) columns, instead of {2} columns as per header");
            return this;
        }
        public CrozzleValidator AreCellsValidAlphabet()
        {
            for (int i = 0; i < crozzle.Width; i++)
            {
                for (int j = 0; j < crozzle[i].Length; j++)
                {
                    string Message = null;
                    switch (crozzle[i, j])
                    {
                        case '\t':
                            Message = string.Format("cell [{0},{1}] contains ({2}), it should only contain exactly 1 alphabetic character", i, "tab", crozzle[i, j]);
                            break;
                        default:
                            if (!char.IsLetter(crozzle[i, j]) && !char.IsWhiteSpace(crozzle[i, j]))
                            {
                                Message = string.Format( "cell [{0},{1}] contains ({2}), it should only contain exactly 1 alphabetic character",  i, j, crozzle[i, j]);
                            }
                            break;
                    }
                    if (Message != null)
                    {
                        LogList.Add(new LogMessage()
                        {
                            Level = Level.Error,
                            Location = "Cell",
                            TextMessage = Message
                        });
                    }
                }
            }

//            for (int i = 0; i < crozzle.Width; i++)
//            {
//                if (crozzle[i].Length != columns)
//                {
//                    LogList.Add(new LogMessage()
//                    {
//                        Level = Level.Error,
//                        Location = "Columns",
//                        TextMessage = string.Format("row {0} contains ({1}) columns. instead of {2} columns", i, crozzle[i].Length, columns)
//                    });
//                }
//            }
            return this;
        }

    }

    public class ConstraintValidator : Validator
    {
        private List<Word> WordsFromCrozzle;
        public ConstraintValidator(List<Word> words, string location)
        {
            WordsFromCrozzle = words;
            LogList = new List<LogMessage>();
            this.location = location;
        }
        public ConstraintValidator IntersectMoreThanOne()
        {
            foreach (Word word in WordsFromCrozzle)
            {
                if (word.IntersectWords.Count < 1)
                {
                    LogList.Add(new LogMessage()
                    {
                        Level = Level.Error,
                        Location = location,
                        TextMessage = string.Format("{0} intersects {1} not (1 or 2 words)", word.ToString(), word.IntersectWords.Count)
                    });
                }
            }
            return this;
        }

        public ConstraintValidator IntersectLessThanTwo()
        {
            foreach (Word word in WordsFromCrozzle)
            {
                if (word.IntersectWords.Count < 1 || word.IntersectWords.Count > 2)
                {
                    LogList.Add(new LogMessage()
                    {
                        Level = Level.Error,
                        Location = location,
                        TextMessage =
                            string.Format("{0} intersects {1} not (1 or 2 words)", word.ToString(),
                                word.IntersectWords.Count)
                    });
                }
            }
            return this;
        }

        private void SearchNoGapWord(Word word, List<Position> positions, Direction direction)
        {
            foreach (Word searchWord in WordsFromCrozzle.Where(d => d.Direction == direction))
            {
                if (searchWord == word) continue; 
                List<Position> searchWordPos = searchWord.CharacterList
                    .Select(w => w.Position).ToList();
                List<Position> found = searchWordPos.Intersect(positions).ToList();
                if (found.Count > 0)
                {
                    foreach (Position position in found)
                    {
                        LogList.Add(new LogMessage()
                        {
                            Level = Level.Error,
                            Location = location,
                            TextMessage =
                                string.Format("No gap({0}) between {1} and {2}",direction, word, searchWord,
                                    word.IntersectWords.Count)
                        });
                    }
                }
            }
        }
        /// <summary>
        /// This method could use some refactoring, due to time constraints I will leave it as it is for now.
        /// </summary>
        /// <returns></returns>
        public ConstraintValidator ValidateNoGap()
        {
            foreach (Word word in WordsFromCrozzle)
            {
                List<Position> positions = new List<Position>();
                switch (word.Direction)
                {
                    case Direction.Horizontal:
                        var firstH = word.CharacterList.FirstOrDefault();
                        for (int i = -1; i < 2; i++)
                        {
                            positions.Add(new Position()
                            {
                                Height = firstH.Position.Height + i,
                                Width = firstH.Position.Width - 1
                            });
                        }
                        var lastH = word.CharacterList.LastOrDefault();
                        for (int i = -1; i < 2; i++)
                        {
                            positions.Add(new Position()
                            {
                                Height = lastH.Position.Height + i,
                                Width = lastH.Position.Width + 1
                            });
                        }
                        foreach (Character character in word.CharacterList)
                        {
                            positions.Add(new Position()
                            {
                                Height = character.Position.Height - 1,
                                Width = character.Position.Width
                            });
                            positions.Add(new Position()
                            {
                                Height = character.Position.Height + 1,
                                Width = character.Position.Width
                            });
                        }
                        SearchNoGapWord(word, positions, word.Direction);
                        break;
                    case Direction.Vertical:
                        var firstV = word.CharacterList.FirstOrDefault();
                        for (int i = -1; i < 2; i++)
                        {
                            positions.Add(new Position()
                            {
                                Height = firstV.Position.Height + 1,
                                Width = firstV.Position.Width + i
                            });
                        }
                        var lastV = word.CharacterList.LastOrDefault();
                        for (int i = -1; i < 2; i++)
                        {
                            positions.Add(new Position()
                            {
                                Height = lastV.Position.Height - 1,
                                Width = lastV.Position.Width + i
                            });
                        }
                        foreach (Character character in word.CharacterList)
                        {
                            positions.Add(new Position()
                            {
                                Height = character.Position.Height,
                                Width = character.Position.Width - 1
                            });
                            positions.Add(new Position()
                            {
                                Height = character.Position.Height,
                                Width = character.Position.Width + 1
                            });
                        }
                        SearchNoGapWord(word, positions, word.Direction);

                        break;
                }
            }
            return this;
        }
    }

}
