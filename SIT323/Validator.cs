using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIT323.Models;

namespace SIT323
{
    /// <summary>
    ///     Abstract base class of Validator
    ///     Implements Ilogger interface
    ///     Some validator methods uses builder pattern to chain methods.
    /// </summary>
    public abstract class Validator : ILogger
    {
        protected string Location;

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
        ///     Check if string is is empty
        /// </summary>
        /// <param name="s">string to be checked for</param>
        /// <returns>true if is empty </returns>
        protected bool IsEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage = "is empty"
                });
                return true;
            }
            return false;
        }
    }

    /// <summary>
    ///     Validator for string
    ///     Inherited from abstract Validator class
    /// </summary>
    public class StringValidtor : Validator
    {
        private string _value;

        /// <summary>
        ///     Constructor for string Validator
        ///     Checks for string if IsEmpty and IsAlphabetic first
        /// </summary>
        /// <param name="s">string to be validated for</param>
        /// <param name="location">location supplied by caller</param>
        public StringValidtor(string s, string location)
        {
            LogList = new List<LogMessage>();
            Location = location;

            IsEmpty(s);
            IsAlphabetic(s);
            _value = s;
        }

        /// <summary>
        ///     IsAlphabetic StringValidtor
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>true if indeed</returns>
        private bool IsAlphabetic(string s)
        {
            //if (s.Any(x => !char.IsLetter(x)))
            if (!Regex.IsMatch(s, @"^[a-zA-Z]*$"))
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage = string.Format("word value ({0}) is not an alphabetic", s)
                });
                return false;
            }
            return true;
        }
    }

    /// <summary>
    ///     Validator for Int
    ///     Inherited from abstract Validator class
    /// </summary>
    public class IntValidator : Validator
    {
        protected int Value = -1;

        /// <summary>
        ///     Constructor for int Validator
        ///     Checks for int if IsEmpty and parse it
        /// </summary>
        /// <param name="s"></param>
        /// <param name="location"></param>
        public IntValidator(string s, string location)
        {
            LogList = new List<LogMessage>();
            Location = location;

            IsEmpty(s);
            if (IsInt(s)) int.TryParse(s, out Value);
        }

        /// <summary>
        ///     method to check if string is a int
        /// </summary>
        /// <param name="s">string from int :/</param>
        /// <returns>true if it is</returns>
        private bool IsInt(string s)
        {
//            if (s.Any(x => !char.IsDigit(x)))
            if(!Regex.IsMatch(s, @"^\d*$"))
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage = string.Format("value ({0}) is not an integer", s)
                });
                return false;
            }
            return true;
        }

        /// <summary>
        ///     method to check if int is in range
        /// </summary>
        /// <param name="min">min supplied</param>
        /// <param name="max">max supplied</param>
        /// <returns>return this for chain calling</returns>
        public IntValidator IsInRange(int min, int max)
        {
            if (Value == -1) return this;
            if (Value < min || Value > max)
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage = string.Format("size value ({0}) is not in range", Value)
                });
            return this;
        }
    }

    /// <summary>
    ///     Validator for wordlist
    ///     Inherited from abstract Validator class
    /// </summary>
    /// <typeparam name="T">Type of word list</typeparam>
    public class WordListValidator<T> : Validator
    {
        private readonly List<T> _list;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="l">list of T</param>
        /// <param name="location">string location</param>
        public WordListValidator(List<T> l, string location)
        {
            _list = l;
            LogList = new List<LogMessage>();
            Location = location;

            IsEmpty(string.Join(",", _list.ToArray()));
        }

        /// <summary>
        ///     Validate if list has certain amount of T
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns>return this for chain calling</returns>
        public WordListValidator<T> IsInRange(int min, int max)
        {
            if (_list.Count == 0) return this;
            if (_list.Count < min || _list.Count > max)
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage =
                        string.Format("{0} contains ({1}) words. Expecting {2} to {3} words", Location, _list.Count, min,
                            max)
                });
            return this;
        }
    }

    /// <summary>
    ///     CrozzleValidator class validate Crozzle object
    /// </summary>
    public class CrozzleValidator : Validator
    {
        private readonly Crozzle _crozzle;

        /// <summary>
        ///     Constructor takes Crozzle object
        /// </summary>
        /// <param name="c"></param>
        public CrozzleValidator(Crozzle c)
        {
            LogList = new List<LogMessage>();
            _crozzle = c;
        }

        /// <summary>
        ///     Check if row and columns of crozzle match specificed value passed in.
        /// </summary>
        /// <param name="row">number of row</param>
        /// <param name="columns">number of columns</param>
        /// <param name="rowMsg">rop error message if not match</param>
        /// <param name="columnsMsg">column eror message if not match</param>
        private void AreCellsSizeCorrect(int row, int columns, string rowMsg, string columnsMsg)
        {
            if (_crozzle.Width != row)
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = "Row",
                    TextMessage = string.Format(rowMsg, _crozzle.Width, row)
                });
            }
            for (var i = 0; i < _crozzle.Width; i++)
            {
                if (_crozzle[i].Length != columns)
                {
                    LogList.Add(new LogMessage
                    {
                        Level = Level.Error,
                        Location = "Columns",
                        TextMessage = string.Format(columnsMsg, i, _crozzle[i].Length, columns)
                    });
                }
            }
        }

        /// <summary>
        ///     Call AreCellsSizeCorrect with custom error message according to requirement
        /// </summary>
        /// <param name="row">row count</param>
        /// <param name="columns">columns count</param>
        /// <returns>return this for chain calling</returns>
        public CrozzleValidator AreCellsSizeCorrectAccordingToRequirement(int row, int columns)
        {
            AreCellsSizeCorrect(row, columns, "file contains ({0}) rows, instead of {1} rows as per requirement",
                "row {0} contains ({1}) columns, instead of {2} columns as per requirement");
            return this;
        }

        /// <summary>
        ///     Call AreCellsSizeCorrect with custom error message according header of wordlist
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columns"></param>
        /// <returns>return this for chain calling</returns>
        public CrozzleValidator AreCellsSizeCorrectAccordingToHeader(int row, int columns)
        {
            AreCellsSizeCorrect(row, columns, "file contains ({0}) rows, instead of {1} rows as per header",
                "row {0} contains ({1}) columns, instead of {2} columns as per header");
            return this;
        }

        /// <summary>
        ///     Validate if each cell is Alphabet
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public CrozzleValidator AreCellsValidAlphabet()
        {
            for (var i = 0; i < _crozzle.Width; i++)
            {
                for (var j = 0; j < _crozzle[i].Length; j++)
                {
                    string message = null;
                    switch (_crozzle[i, j])
                    {
                        case '\t':
                            message =
                                string.Format(
                                    "cell [{0},{1}] contains ({2}), it should only contain exactly 1 alphabetic character",
                                    i, "tab", _crozzle[i, j]);
                            break;
                        default:
                            if (!char.IsLetter(_crozzle[i, j]) && !char.IsWhiteSpace(_crozzle[i, j]))
                            {
                                message =
                                    string.Format(
                                        "cell [{0},{1}] contains ({2}), it should only contain exactly 1 alphabetic character",
                                        i, j, _crozzle[i, j]);
                            }
                            break;
                    }
                    if (message != null)
                    {
                        LogList.Add(new LogMessage
                        {
                            Level = Level.Error,
                            Location = "Cell",
                            TextMessage = message
                        });
                    }
                }
            }
            return this;
        }
    }

    public class ConstraintValidator : Validator
    {
        private readonly List<Word> _wordsFromCrozzle;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="words">list of word to be validated</param>
        /// <param name="location">location</param>
        public ConstraintValidator(List<Word> words, string location)
        {
            _wordsFromCrozzle = words;
            LogList = new List<LogMessage>();
            Location = location;
        }

        /// <summary>
        ///     Check if word has more than one other intersecting words
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public ConstraintValidator AreWordsIntersectingMoreThanOnce()
        {
            foreach (var word in _wordsFromCrozzle)
            {
                if (word.IntersectWords.Count < 1)
                {
                    word.IsValid = false;
                    LogList.Add(new LogMessage
                    {
                        Level = Level.Error,
                        Location = Location,
                        TextMessage =
                            string.Format("{0} intersects {1} not (1 or 2 words)", word, word.IntersectWords.Count)
                    });
                }
                word.IsValid = true;
            }
            return this;
        }

        /// <summary>
        ///     Check if word has one or two other intersecting words
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public ConstraintValidator AreWordsIntersectingOnceOrTwice()
        {
            foreach (var word in _wordsFromCrozzle)
            {
                if (word.IntersectWords.Count < 1 || word.IntersectWords.Count > 2)
                {
                    word.IsValid = false;
                    LogList.Add(new LogMessage
                    {
                        Level = Level.Error,
                        Location = Location,
                        TextMessage =
                            string.Format("{0} intersects {1} not (1 or 2 words)", word,
                                word.IntersectWords.Count)
                    });
                }
                word.IsValid = true;
            }
            return this;
        }

        /// <summary>
        ///     This private method is called by ValidateNoGap()
        ///     to loop thought each word to find any one grid space between same direction word
        /// </summary>
        /// <param name="word">word object</param>
        /// <param name="positions">list of positions</param>
        /// <param name="direction">director</param>
        private void SearchNoGapWord(Word word, List<Position> positions, Direction direction)
        {
            foreach (var searchWord in _wordsFromCrozzle.Where(d => d.Direction == direction))
            {
                if (searchWord == word) continue;
                var searchWordPos = searchWord.CharacterList
                    .Select(w => w.Position).ToList();
                var found = searchWordPos.Intersect(positions).ToList();
                if (found.Count > 0)
                {
                    foreach (var position in found)
                    {
                        word.IsValid = false;
                        LogList.Add(new LogMessage
                        {
                            Level = Level.Error,
                            Location = Location,
                            TextMessage =
                                string.Format("No gap({0}) between {1} and {2}", direction, word, searchWord,
                                    word.IntersectWords.Count)
                        });
                    }
                }
            }
        }

        /// <summary>
        ///     check if there is no gap in a word
        ///     This method could use some refactoring, due to time constraints I will leave it as it is for now.
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public ConstraintValidator ValidateNoGap()
        {
            foreach (var word in _wordsFromCrozzle)
            {
                var positions = new List<Position>();
                switch (word.Direction)
                {
                    case Direction.Horizontal:
                        var firstH = word.CharacterList.FirstOrDefault();
                        for (var i = -1; i < 2; i++)
                        {
                            positions.Add(new Position
                            {
                                Height = firstH.Position.Height + i,
                                Width = firstH.Position.Width - 1
                            });
                        }
                        var lastH = word.CharacterList.LastOrDefault();
                        for (var i = -1; i < 2; i++)
                        {
                            positions.Add(new Position
                            {
                                Height = lastH.Position.Height + i,
                                Width = lastH.Position.Width + 1
                            });
                        }
                        foreach (var character in word.CharacterList)
                        {
                            positions.Add(new Position
                            {
                                Height = character.Position.Height - 1,
                                Width = character.Position.Width
                            });
                            positions.Add(new Position
                            {
                                Height = character.Position.Height + 1,
                                Width = character.Position.Width
                            });
                        }
                        SearchNoGapWord(word, positions, word.Direction);
                        break;
                    case Direction.Vertical:
                        var firstV = word.CharacterList.FirstOrDefault();
                        for (var i = -1; i < 2; i++)
                        {
                            positions.Add(new Position
                            {
                                Height = firstV.Position.Height + 1,
                                Width = firstV.Position.Width + i
                            });
                        }
                        var lastV = word.CharacterList.LastOrDefault();
                        for (var i = -1; i < 2; i++)
                        {
                            positions.Add(new Position
                            {
                                Height = lastV.Position.Height - 1,
                                Width = lastV.Position.Width + i
                            });
                        }
                        foreach (var character in word.CharacterList)
                        {
                            positions.Add(new Position
                            {
                                Height = character.Position.Height,
                                Width = character.Position.Width - 1
                            });
                            positions.Add(new Position
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

        /// <summary>
        ///     check if words are connected/interected
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public ConstraintValidator AreWordsConnected()
        {
            var words = _wordsFromCrozzle.ToList();
            var connectedGroup = new List<List<Word>>();
            for (var i = 0; i < words.Count; i++)
            {
                if (words[i].Visited) continue;
                var group = new List<Word>();
                DfsOnWords(words[i], group, words);
                connectedGroup.Add(group);
            }

            if (connectedGroup.Count > 1)
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location + "Ex3",
                    TextMessage =
                        string.Format("This crozzle contains ({0}) group, instead of 1", connectedGroup.Count)
                });
            }
            return this;
        }

        /// <summary>
        ///     Finds connected words using depth-first-search
        /// </summary>
        /// <param name="word">word object</param>
        /// <param name="group">list of grouped/connected words</param>
        /// <param name="words">list of orginal words</param>
        private void DfsOnWords(Word word, List<Word> group, List<Word> words)
        {
            if (group.Contains(word)) return;
            group.Add(word);
            word.Visited = true;
            foreach (var i in word.IntersectWords)
            {
                if (!i.Visited) DfsOnWords(i, group, words);
            }
            foreach (var i in words)
            {
                if (!i.Visited && i.IntersectWords.Contains(word)) DfsOnWords(i, group, words);
            }
        }
    }

    /// <summary>
    ///     ConstraintWithWordListValidator class
    /// </summary>
    public class ConstraintWithWordListValidator : Validator
    {
        private readonly Wordlist _wordList;
        private readonly List<Word> _wordsFromCrozzle;
        private readonly List<string> _wordsInString;

        public ConstraintWithWordListValidator(List<Word> words, Wordlist wordlist, string location)
        {
            _wordList = wordlist;
            _wordsFromCrozzle = words;
            LogList = new List<LogMessage>();
            Location = location;

            _wordsInString = new List<string>();
            foreach (var chars in _wordsFromCrozzle.Select(w => w.CharacterList))
            {
                _wordsInString.Add(string.Join("", chars.Select(a => a.Alphabetic).ToArray()));
            }
        }

        /// <summary>
        ///     Check if there is no dupe words in a crozzle
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public ConstraintWithWordListValidator AreThereNoDupeWords()
        {
            var dupes = _wordsInString.GroupBy(w => w)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);
            foreach (var dupe in dupes)
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage =
                        string.Format("{0} exists more than once", dupe)
                });
            }
            return this;
        }

        /// <summary>
        ///     Check if words on crozzle are on wordlist
        /// </summary>
        /// <returns>return this for chain calling</returns>
        public ConstraintWithWordListValidator AreWordsOnWordList()
        {
            var gatecrashers = new HashSet<string>(_wordsInString);
            gatecrashers.ExceptWith(_wordList.WordList);
            foreach (var gatecrasher in gatecrashers)
            {
                LogList.Add(new LogMessage
                {
                    Level = Level.Error,
                    Location = Location,
                    TextMessage =
                        string.Format("{0} is not in the wordlist", gatecrasher)
                });
            }
            return this;
        }
    }
}