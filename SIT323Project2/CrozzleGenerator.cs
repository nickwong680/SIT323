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

        public CrozzleGenerator(CrozzleProject2 crozzle, Wordlist wordlist)
        {
            Crozzle = crozzle;
            _wordlist = wordlist;
            _difficulty = crozzle.Level;
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
            foreach (var word in words.OrderByDescending(w => w.Score).ThenBy(w => w.CharacterList.Count))
            {
                str.Add(word.ToString());
            }
            return str;
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
                List<SpanWithCharater> spans = Crozzle.FindInterectableWords();

                var match = new MatchSpanToWord(spans, WordsNotAddedList);
                var matches = match.MatchAndOrderByPints();

                if (matches.Count == 0)
                {
                    if (_difficulty == Difficulty.Extreme)
                    {
                        break;
                    }
                    else
                    {
                        if (!InsertNewWord()) break;
                    }
                }
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
                    word.Direction = matched.SpanWithCharater.Direction;

                    var pos = matched.SpanWithCharater.Position;
                    if (matched.SpanWithCharater.Direction == Direction.Vertical)
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

            PostCheck();

            Console.WriteLine(Crozzle.ToString());
        }

        private void PostCheck()
        {
            List<Word> wordsToBeRemoved = new List<Word>();
            if (_difficulty == Difficulty.Easy || _difficulty == Difficulty.Medium)
            {
                wordsToBeRemoved.AddRange( Crozzle.Wordlist.Where(w => w.IntersectWords.Count == 0));
                foreach (Word word in wordsToBeRemoved)
                {
                    Crozzle.RemoveWord(word);
                    Crozzle.Wordlist.Remove(word);
                }
            }
        }



        private bool InsertNewWord()
        {
            List<Span> spans = Crozzle.FindEmptySpans();
            if (spans == null) return false; 

            Span span = null;
            string wordStr = null;
            foreach (string wStr in WordsNotAddedList)
            {
                span = spans.FirstOrDefault(s => s.Length >= wStr.Length - 1);
                wordStr = wStr;
                if (span != null) break;
            }
            if (span == null) return false; 
            Word word = CreateWordWithPoints(wordStr);
            word.Direction = span.Direction;

            Adder = new AddWordToGrid(this, word, span.Position);
            if(Adder.Added == true) WordsNotAddedList.Remove(word.ToString());
            return true;
        }


    }
}