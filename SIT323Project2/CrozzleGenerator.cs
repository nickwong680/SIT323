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

        private Word CreateWordWithPoints(Direction dir, string wordStr)
        {
            PointScheme pointScheme;
            switch (_difficulty)
            {
                 case Difficulty.Easy:
                    pointScheme = PointScheme.OneEach;
                    break;
                case Difficulty.Medium:
                    pointScheme = PointScheme.Incremental;
                    break;
                case Difficulty.Hard:
                    pointScheme = PointScheme.IncrementalWithBonusPerWord;
                    break;
                case Difficulty.Extreme:
                default:
                    pointScheme = PointScheme.CustomWithBonusPerIntersection;
                    break;
            }
            List<Character> scores = Score.PointsMatrix(pointScheme);
            Word word = new Word(dir, wordStr);
            foreach (Character character in word.CharacterList)
            {
                character.Score = scores.First(c => c.Alphabetic == character.Alphabetic).Score;
            }
            return word;
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
                word = CreateWordWithPoints(Direction.Horizontal, WordsNotAddedList.FirstOrDefault());
                Adder = new AddWordToGrid(this, word, (_wordlist.Height/2),
                    (_wordlist.Width/2) - (word.CharacterList.Count/2));
                WordsNotAddedList.Remove(word.ToString());
            }

            do
            {
                if (Crozzle.Wordlist.LastOrDefault().ToString() == "EXPORT")
                {
                    int tt = 0;
                }

                List<Span> spans = Crozzle.InterectableWords();

                var match = new MatchSpanToWord(spans, WordsNotAddedList);
                List<WordMatch> matches = match.Match();
                if(matches.Count == 0) break;
                do
                {
                    WordMatch matched = null;
                    foreach (WordMatch wordMatch in matches)
                    {
                        if (wordMatch.Span.Direction != Crozzle.Wordlist.LastOrDefault().Direction)
                        {
                            matched = wordMatch;
                            break;
                        }
                    }
                    if (matched == null) matched = matches.FirstOrDefault();


                    word = CreateWordWithPoints(matched.Span.Direction, matched.Word);
                    Position pos = matched.Span.Position;
                    if (matched.Span.Direction == Direction.Vertical)
                    {
                        pos.Height -= matched.MatchIndex;
                    }
                    else
                    {
                        pos.Width -= matched.MatchIndex;
                    }
                    Adder = new AddWordToGrid(this, word, pos);
                    if (Adder.Added == false) matches.Remove(matched);
                } while (Adder.Added == false);
                WordsNotAddedList.Remove(word.ToString());


                Console.WriteLine(String.Join(",", Crozzle.Wordlist));
                Console.WriteLine(Crozzle.ToString());

                //if (Crozzle.Wordlist.Count > 20) break;
            } while (true);

            Console.WriteLine(Crozzle.ToString());
        }

        private void Add(Word word, WordMatch match)
        {
            Adder = new AddWordToGrid(this, word, (_wordlist.Height/2),
                (_wordlist.Width/2) - (word.CharacterList.Count/2));
            Crozzle.Wordlist.Add(word);
            WordsNotAddedList.Remove(word.ToString());
        }
    }
}