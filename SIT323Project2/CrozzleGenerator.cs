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
        private readonly Difficulty _difficulty;
        private readonly Wordlist _wordlist;
        public readonly CrozzleProject2 Crozzle;

        public CrozzleGenerator(CrozzleProject2 crozzle, Wordlist wordlist, Difficulty difficulty)
        {
            Crozzle = crozzle;
            _wordlist = wordlist;
            _difficulty = difficulty;
            WordsNotAddedList = OrderWordListByCountAndPoint(wordlist);
        }

        public List<string> WordsNotAddedList { get; private set; }

        public AddWordToGrid Adder { get; private set; }

        public Wordlist Wordlist
        {
            get { return _wordlist; }
        }

        private List<string> OrderWordListByCountAndPoint(Wordlist wordlist)
        {
            var str = new List<string>();
            var words = new List<Word>();
            foreach (var w in wordlist.WordList)
            {
                words.Add(CreateWordWithPoints(w));
            }
            foreach (var word in words.OrderByDescending(w => w.CharacterList.Count).ThenBy(w => w.Score))
            {
                str.Add(word.ToString());
            }
            return str;
        }

        public void FindInterectableWords()
        {
            var spans = Crozzle.InterectableWords();
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

        private Word CreateWordWithPoints(string wordStr)
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
            var scores = Score.PointsMatrix(pointScheme);
            var word = new Word(wordStr);
            var wordScore = 0;

            foreach (var character in word.CharacterList)
            {
                character.Score = scores.First(c => c.Alphabetic == character.Alphabetic).Score;
                wordScore += character.Score;
            }
            word.Score = wordScore;
            return word;
        }

        /// <summary>
        ///     Start by placing first word (the most longest word) to center of the crozzle
        /// </summary>
        public void PlaceWordsToGrid()
        {
            Word word = null;
            Random random = new Random();
            if (Crozzle.Wordlist.Count == 0)
            {
                int height = random.Next(1, _wordlist.Height - 1);
                word = CreateWordWithPoints(WordsNotAddedList.FirstOrDefault());
                word.Direction = Direction.Horizontal;
                Adder = new AddWordToGrid(this, word, (height),
                    (_wordlist.Width/2) - (word.CharacterList.Count/2));
                WordsNotAddedList.Remove(word.ToString());
            }

            do
            {
                if (Crozzle.Wordlist.LastOrDefault().ToString() == "EXPORT")
                {
                    var tt = 0;
                }
                if (WordsNotAddedList.Count <= 186)
                {
                    var tt = 0;
                }
                var spans = Crozzle.InterectableWords();

                var match = new MatchSpanToWord(spans, WordsNotAddedList);
                var matches = match.MatchAndOrderByPints();
                if (matches.Count == 0) break;

                do
                {
                    if (matches.Count == 0) break;

                    int rint = random.Next(0, (int) (matches.Count * 0.10));
                    var matched = matches[rint];

//                    foreach (WordMatch wordMatch in matches)
//                    {
//                        if (wordMatch.Span.Direction != Crozzle.Wordlist.LastOrDefault().Direction)
//                        {
//                            matched = wordMatch;
//                            break;
//                        }
//                    }
                    if (matched == null) matched = matches.FirstOrDefault();

                    word = CreateWordWithPoints(matched.Word);
                    word.Direction = matched.Span.Direction;

                    var pos = matched.Span.Position;
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

                Console.WriteLine(string.Join(",", Crozzle.Wordlist));
                Console.WriteLine(Crozzle.ToString());

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