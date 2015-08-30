using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIT323.Models;

namespace SIT323
{
    public abstract class Constraints : ILogger
    {
        protected Wordlist Wordlist { get; set; }
        protected Crozzle Crozzle{ get; set; }
        public List<Word> WordsFromCrozzle { get; set; }

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
                        new Position()
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
                        new Position()
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
        /// [S][S] skip
        /// [S][C] skip
        /// [C][C] alloc or add
        /// [C][S] dealloc or skip
        /// </summary>
        /// <param name="current"></param>
        /// <param name="next"></param>
        /// <param name="word"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Word ProcessCell(Character current, char next, Word word, Direction direction)
        {
            //if current cell and the next cell is letter
            if (char.IsLetter(current.Alphabetic) && char.IsLetter(next))
            {
                if (word == null)
                {
                    return new Word(direction, current);
                }
                else
                {
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
            }
            else if (char.IsLetter(current.Alphabetic) && char.IsWhiteSpace(next))
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
                        SearchIntersectingWords(word, positions, word.Direction);
                        break;
                    case Direction.Vertical:
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
                        SearchIntersectingWords(word, positions, word.Direction);
                        break;

                }
            }
        }


        public List<LogMessage> LogList { get; set; }
        public string LogListInString()
        {
            return string.Join(Environment.NewLine, LogList.Select(l => l.ToString()));
        }
    }

    public class EasyConstraints : Constraints
    {
        public EasyConstraints(Crozzle c, Wordlist w) : base(c,w)
        {
            LogList.AddRange(new ConstraintValidator(WordsFromCrozzle, "EasyConstraints")
                .AreWordsIntersectingOnceOrTwice()
                .ValidateNoGap()
                .LogList
                );
        }
    }
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
