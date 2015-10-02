using System;
using System.Collections.Generic;
using System.Linq;
using SIT323;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{
    public class CrozzleGenerator
    {
        public readonly CrozzleProject2 Crozzle;

        private readonly Difficulty _difficulty;
        private readonly Wordlist _wordlist;
        private List<string> _wordsNotAddedList;

        public CrozzleGenerator(CrozzleProject2 crozzle, Wordlist wordlist, Difficulty difficulty)
        {
            this.Crozzle = crozzle;
            this._wordlist = wordlist;
            this._difficulty = difficulty;
            this._wordsNotAddedList = new List<string>(wordlist.WordList.OrderByDescending(w => w.Count()));
        }

        public List<string> WordsNotAddedList
        {
            get { return _wordsNotAddedList; }
        }

        public AddWordToGrid Adder { get; private set; }

        public Wordlist Wordlist
        {
            get { return _wordlist; }
        }

        public void FindInterectableWords()
        {
            List<Span> spans = Crozzle.InterectableWords();
        }

        private Dictionary<string, int> FindWordsInterectableWords(Word word)
        {
            var words = new Dictionary<string, int>();
            for (var i = 0; i < word.CharacterList.Count; i++)
            {
                var interects = WordsNotAddedList.Where(w => w.Contains(word.CharacterList[i].Alphabetic));
                foreach (var interect in interects)
                {
                    words.Add(interect, i);
                }
            }
            return words;
        }
        /// <summary>
        /// Start by placing first word (the most longest word) to center of the crozzle
        /// 
        /// </summary>
        public void PlaceWordsToGrid()
        {
            Word word;
            if (Crozzle.Wordlist.Count == 0)
            {
                word = new Word(Direction.Horizontal, WordsNotAddedList.FirstOrDefault());
                Adder = new AddWordToGrid(this, word, (_wordlist.Height/2),
                    (_wordlist.Width/2) - (word.CharacterList.Count/2));
                Crozzle.Wordlist.Add(word);
                WordsNotAddedList.Remove(word.ToString());
            }
            do
            {
                List<Span> spans = Crozzle.InterectableWords();
                var span = new MatchSpanToWord(spans, WordsNotAddedList);



            } while (WordsNotAddedList.Count > 0);

            Console.WriteLine(Crozzle.ToString());
        }
    }
}