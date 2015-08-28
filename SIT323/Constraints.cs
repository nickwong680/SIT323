using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIT323.Models;

namespace SIT323
{
    public abstract class Constraints : ILogger
    {
        public List<LogMessage> LogList{ get; set; }
        protected Crozzle Crozzle{ get; set; }
        public List<Word> WordsFromWordList { get; set; }

        protected Constraints(Crozzle c)
        {
            Crozzle = c;
            WordsFromWordList = new List<Word>();
            MakeCrozzleWords();

        }

        private void MakeCrozzleWords()
        {
            //Horizontal Traversal
            for (int i = 0; i < Crozzle.Width; i++)
            {
                List<Word> wordList = new List<Word>();
                for (int j = 0; j < Crozzle[i].Length - 1; j++)
                {
                    Character character = new Character(
                        Crozzle[i, j],
                        new Position()
                        {
                            Height = i,
                            Width = j
                        });
                    Word word = wordList.LastOrDefault();
                    Word tmpWord = ProcessCell(character, Crozzle[i, j + 1], word, Direction.Horizontal);
                    if (tmpWord != null && word != tmpWord)
                    {
                        wordList.Add(tmpWord);
                    }
                }
                WordsFromWordList.AddRange(wordList);
            }

            //Vertical Traversal
            for (int i = 0; i < Crozzle.Height; i++)
            {
                List<Word> wordList = new List<Word>();
                for (int j = 0; j < Crozzle.Width - 1; j++)
                {
                    Character character = new Character(
                        Crozzle[j, i],
                        new Position()
                        {
                            Height = j,
                            Width = i
                        });
                    Word word = wordList.LastOrDefault();
                    Word tmpWord = ProcessCell(character, Crozzle[j + 1 , i], word, Direction.Vertical);
                    if (tmpWord != null && word != tmpWord)
                    {
                        wordList.Add(tmpWord);
                    }
                }
                WordsFromWordList.AddRange(wordList);
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
        public Constraints Intersect(int max)
        {

            return this;
        }
    }

    public class EasyConstraints : Constraints
    {
        public EasyConstraints(Crozzle c) : base(c)
        {
        }
    }
}
