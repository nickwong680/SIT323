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

        protected Constraints()
        {
            WordsFromWordList = new List<Word>();
        }

        private void ProcessWords()
        {
            //Horizontal Scan
            for (int i = 0; i < Crozzle.Width; i++)
            {
                for (int j = 0; j < Crozzle[i].Length; j++)
                {
                    Character character = new Character(
                        Crozzle[i, j],
                        new Position()
                        {
                            Height = i,
                            Width = j
                        });
                    Word word = WordsFromWordList.LastOrDefault();
                    Word tmpWord = ProcessCell(character, Crozzle[i, j + 1], word, Direction.Horizontal);
                    if (tmpWord != null && word != tmpWord)
                    {
                        WordsFromWordList.Add(tmpWord);
                    }
                }
            }
        }

        private Word ProcessCell(Character current, char next, Word word, Direction direction)
        {
            Word newWord;
            //if current cell and the next cell is letter
            if (char.IsLetter(current.Alphabetic) && char.IsLetter(next))
            {
                if (word == null)
                {
                    return new Word(direction, current);
                }
                else
                {
                    word.CharacterList.Add(current);
                    return word;
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
        public EasyConstraints(Wordlist wlist)
        {
           
        }
    }
}
