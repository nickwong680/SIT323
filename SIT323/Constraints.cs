using System;
using System.Collections.Generic;
using System.Linq;
using SIT323.Models;

namespace SIT323
{
    /// <summary>
    ///     Abstract class of Constraints
    ///     Implements Ilogger interface
    /// </summary>
    public abstract class Constraints : ILogger
    {
        /// <summary>
        ///     Base class constructor
        ///     Calls AreWordsOnWordList() and AreThereNoDupeWords() that are applicable to all subclasses
        /// </summary>
        /// <param name="c">Crozzle</param>
        /// <param name="w">Wordlist</param>
        protected Constraints(Crozzle c, Wordlist w)
        {
            Wordlist = w;
            Crozzle = c;
            WordsFromCrozzle = new List<Word>();
            MakeCrozzleWords();
            Intersect();

            LogList = new List<LogMessage>();
            LogList.AddRange(new ConstraintWithWordListValidator(WordsFromCrozzle, Wordlist, "Constraint")
                .AreWordsOnWordList()
                .AreThereNoDupeWords()
                .LogList);
        }

        /// <summary>
        ///     Wordlist constructed from Wordlist class
        /// </summary>
        protected Wordlist Wordlist { get; set; }

        /// <summary>
        ///     Crozzle constructed from Crozzle class
        /// </summary>
        protected Crozzle Crozzle { get; set; }

        /// <summary>
        ///     List of words to be created
        /// </summary>
        public List<Word> WordsFromCrozzle { get; set; }

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

        private void MakeCrozzleWords()
        {
            //Horizontal Traversal
            for (int i = 0; i < Crozzle.Width; i++)
            {
                List<Word> wordList = new List<Word>();
                for (int j = 0; j < Crozzle[i].Length; j++)
                {
                    Character character = new Character(
                        Crozzle[i, j],
                        new Position
                        {
                            Height = i,
                            Width = j
                        });
                    Word word = wordList.LastOrDefault();
                    char next = Crozzle[i, j + 1];
                    Word tmpWord = ProcessCell(character, next, word, Direction.Horizontal);
                    if (tmpWord != null && word != tmpWord)
                    {
                        wordList.Add(tmpWord);
                    }
                }
                WordsFromCrozzle.AddRange(wordList);
            }

            //Vertical Traversal
            for (int i = 0; i < Crozzle.Height; i++)
            {
                List<Word> wordList = new List<Word>();
                for (int j = 0; j < Crozzle.Width; j++)
                {
                    Character character = new Character(
                        Crozzle[j, i],
                        new Position
                        {
                            Height = j,
                            Width = i
                        });
                    Word word = wordList.LastOrDefault();
                    char next = Crozzle[j + 1, i];
                    Word tmpWord = ProcessCell(character, next, word, Direction.Vertical);
                    if (tmpWord != null && word != tmpWord)
                    {
                        wordList.Add(tmpWord);
                    }
                }
                WordsFromCrozzle.AddRange(wordList);
            }
        }

        /// <summary>
        ///     [S][S] skip
        ///     [S][C] skip
        ///     [C][C] alloc or add
        ///     [C][S] dealloc or skip
        /// </summary>
        /// <param name="current">Current Character</param>
        /// <param name="next">next Character in char</param>
        /// <param name="word">Current word</param>
        /// <param name="direction">Direction - Horizontal or Vertical</param>
        /// <returns></returns>
        private Word ProcessCell(Character current, char next, Word word, Direction direction)
        {
            //If [C][C] if current cell and the next cell is letter 
            //Else [C][S] dealloc or skip
            if (char.IsLetter(current.Alphabetic) && char.IsLetter(next))
            {
                if (word == null)
                {
                    return new Word(direction, current);
                }
                Character lastCharacter = word.CharacterList.LastOrDefault();
                switch (direction)
                {
                    case Direction.Horizontal:
                        if (current.Position.Width - lastCharacter.Position.Width == 1)
                        {
                            word.CharacterList.Add(current);
                        }
                        else
                        {
                            word = new Word(direction, current);
                        }
                        break;
                    case Direction.Vertical:
                        if (current.Position.Height - lastCharacter.Position.Height == 1)
                        {
                            word.CharacterList.Add(current);
                        }
                        else
                        {
                            word = new Word(direction, current);
                        }
                        break;
                }
                return word;
            }
            if (char.IsLetter(current.Alphabetic) && (char.IsWhiteSpace(next) || next == default (char)))
            {
                if (word == null) return null;
                Character lastCharacter = word.CharacterList.LastOrDefault();

                if (lastCharacter == null) return null;
                switch (direction)
                {
                    case Direction.Horizontal:
                        if (current.Position.Width - lastCharacter.Position.Width == 1)
                        {
                            word.CharacterList.Add(current);
                            return word;
                        }
                        break;
                    case Direction.Vertical:
                        if (current.Position.Height - lastCharacter.Position.Height == 1)
                        {
                            word.CharacterList.Add(current);
                            return word;
                        }
                        break;
                }
            }
            return null;
        }

        /// <summary>
        ///     Search any Intersecting words in oppsite direction and add to IntersectWords porperty
        /// </summary>
        /// <param name="word">Word that other words may inersect</param>
        /// <param name="positions">list of position to seach for</param>
        /// <param name="direction">direction current word</param>
        private void SearchIntersectingWords(Word word, List<Position> positions, Direction direction)
        {
            foreach (Word searchWord in WordsFromCrozzle.Where(d => d.Direction != direction))
            {
                List<Position> searchWordPos = searchWord.CharacterList
                    .Select(w => w.Position).ToList();
                List<Position> found = searchWordPos.Intersect(positions).ToList();
                if (found.Count > 0) word.IntersectWords.Add(searchWord);
            }
        }

        /// <summary>
        ///     Loop each word from Crozzle and finds any Intersecting words for both directions
        /// </summary>
        public void Intersect()
        {
            foreach (Word word in WordsFromCrozzle)
            {
                List<Position> positions = new List<Position>();
                switch (word.Direction)
                {
                    case Direction.Horizontal:
                        foreach (Character character in word.CharacterList)
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
                        SearchIntersectingWords(word, positions, word.Direction);
                        break;
                    case Direction.Vertical:
                        foreach (Character character in word.CharacterList)
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
                        SearchIntersectingWords(word, positions, word.Direction);
                        break;
                }
            }
        }
    }

    /// <summary>
    ///     EasyConstraints class inherits from Abstract class
    ///     Call applicable validators based on project requirement
    ///     AreWordsIntersectingOnceOrTwice()
    ///     ValidateNoGap()
    /// </summary>
    public class EasyConstraints : Constraints
    {
        public EasyConstraints(Crozzle c, Wordlist w) : base(c, w)
        {
            LogList.AddRange(new ConstraintValidator(WordsFromCrozzle, "EasyConstraints")
                .AreWordsIntersectingOnceOrTwice()
                .ValidateNoGap()
                .LogList
                );
        }
    }

    /// <summary>
    ///     MediumConstraints class inherits from Abstract class
    ///     Call applicable validator based on project requirement
    ///     AreWordsIntersectingOnceOrTwice()
    /// </summary>
    public class MediumConstraints : Constraints
    {
        public MediumConstraints(Crozzle c, Wordlist w)
            : base(c, w)
        {
            LogList.AddRange(new ConstraintValidator(WordsFromCrozzle, "MediumConstraints")
                .AreWordsIntersectingOnceOrTwice()
                .LogList
                );
        }
    }

    /// <summary>
    ///     HardConstraints class inherits from Abstract class
    ///     Call applicable validator based on project requirement
    ///     AreWordsIntersectingOnceOrTwice()
    /// </summary>
    public class HardConstraints : Constraints
    {
        public HardConstraints(Crozzle c, Wordlist w)
            : base(c, w)
        {
            LogList.AddRange(new ConstraintValidator(WordsFromCrozzle, "HardConstraints")
                .AreWordsIntersectingMoreThanOnce()
                .LogList
                );
        }
    }

    /// <summary>
    ///     ExtremeConstraints class inherits from Abstract class
    ///     Call applicable validator based on project requirement
    ///     AreWordsIntersectingMoreThanOnce()
    ///     AreWordsConnected()
    /// </summary>
    public class ExtremeConstraints : Constraints
    {
        public ExtremeConstraints(Crozzle c, Wordlist w)
            : base(c, w)
        {
            LogList.AddRange(new ConstraintValidator(WordsFromCrozzle, "ExtremeConstraints")
                .AreWordsIntersectingMoreThanOnce()
                .AreWordsConnected()
                .LogList
                );
        }
    }
}