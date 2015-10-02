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
        public readonly CrozzleProject2 crozzle;

        private readonly Difficulty difficulty;
        private Wordlist wordlist;

        public CrozzleGenerator(CrozzleProject2 crozzle, Wordlist wordlist, Difficulty difficulty)
        {
            this.crozzle = crozzle;
            this.wordlist = wordlist;
            this.difficulty = difficulty;
            WordsNotAddedList = new List<string>(wordlist.WordList.OrderByDescending(w => w.Count()));
        }

        public List<string> WordsNotAddedList { get; private set; }

        /// <summary>
        /// This method does a few things
        ///     It add word to grid
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void PlaceSingleWordToGrid(Word word, int height, int width)
        {
            AddWordToGrid(word, height, width);
            crozzle.Wordlist.Add(word);
            WordsNotAddedList.Remove(word.ToString());
        }

        /// <summary>
        ///     Overload for PlaceSingleWordToGrid
        /// </summary>
        /// <param name="word"></param>
        /// <param name="pos"></param>
        public void PlaceSingleWordToGrid(Word word, Position pos)
        {
            PlaceSingleWordToGrid(word,  pos.Height, pos.Width);
        }
        /// <summary>
        /// 
        /// </summary>
        public void PlaceWordsToGrid()
        {
            Word word;
            if (crozzle.Wordlist.Count == 0)
            {
                word = new Word(Direction.Horizontal, WordsNotAddedList.FirstOrDefault());
                PlaceSingleWordToGrid(word, (wordlist.Height / 2), (wordlist.Width / 2) - (word.CharacterList.Count / 2));

//                word = new Word(Direction.Horizontal, WordsNotAddedList.FirstOrDefault());
//                PlaceSingleWordToGrid(word, (wordlist.Width / 2) - (word.CharacterList.Count / 2), wordlist.Height / 2);
//            
            }
//            do
//            {
//
//            } while (wordsNotAddedList.Count > 0);

            Console.WriteLine(crozzle.ToString());
        }

        private Dictionary<string, int> FindWordsInterectOn(Word word)
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

        private bool DoesWordFit(Word word, int height, int width)
        {
            if (word.Direction == Direction.Horizontal)
            {
                var span = height;
                for (var i = 0; i < word.CharacterList.Count; i++)
                {
                    var grid = crozzle[span, width];
                    switch (grid.SpannableDirection)
                    {
                        case Direction.Vertical:
                        case Direction.All:
                            return false;
                    }
                    if (grid.Character != '\0' && grid.Character != word.CharacterList[i].Alphabetic)
                    {
                        return false;
                    }
                    span++;
                }
            }
            else
            {
                var span = width;
                for (var i = 0; i < word.CharacterList.Count; i++)
                {
                    var grid = crozzle[height, width];
                    switch (grid.SpannableDirection)
                    {
                        case Direction.Horizontal:
                        case Direction.All:
                            return false;
                    }
                    if (grid.Character != '\0' && grid.Character != word.CharacterList[i].Alphabetic)
                    {
                        return false;
                    }
                    span++;
                }
            }
            return true;
        }


        public void AddWordToGrid(Word word, int height, int width)
        {
            crozzle.Wordlist.Add(word);
            if (word.Direction == Direction.Vertical)
            {
                var span = height;
                for (var i = 0; i < word.CharacterList.Count; i++)
                {
                    var grid = crozzle[span, width];
                    grid.Character = word.CharacterList[i].Alphabetic;
                    word.CharacterList[i].Position = new Position {Height = span, Width = width};
                    grid.VerticalWord = word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Horizontal
                        : Direction.None;
                    span++;
                }
            }
            else
            {
                var span = width;
                for (var i = 0; i < word.CharacterList.Count; i++)
                {
                    var grid = crozzle[height,span];
                    grid.Character = word.CharacterList[i].Alphabetic;
                    word.CharacterList[i].Position = new Position {Height = height, Width = span};
                    grid.HorizontalWord = word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Vertical
                        : Direction.None;
                    span++;
                }
            }
            UpdateGrid(word, height, width);
        }

        /// <summary>
        ///     EEEEEEEE
        ///     EXWORDXE
        ///     EEEEEEEE
        ///     E = Empty
        ///     X = To be Touched
        /// </summary>
        private void UpdateHeadAndTailGrid(Word word, int height, int weight)
        {
            Grid headGrid;
            Grid tailGrid;
            if (word.Direction == Direction.Vertical)
            {
                headGrid = crozzle[height - 1, weight];
                tailGrid = crozzle[height + word.CharacterList.Count, weight];
            }
            else
            {
                headGrid = crozzle[height, weight - 1];
                tailGrid = crozzle[height, weight + word.CharacterList.Count];
            }
            if (headGrid != null) headGrid.SpannableDirection = Direction.None;
            if (tailGrid != null) tailGrid.SpannableDirection = Direction.None;
        }

        /// <summary>
        ///     EEEEEEEE
        ///     EXXXXXXE
        ///     EEWORDEE
        ///     EXXXXXXE
        ///     EEEEEEEE
        ///     E = Empty
        ///     X = To be Touched
        /// </summary>
        private void UpdateSurroundedGrids(Word word, int height, int width)
        {
            for (var i = -1; i < 2; i++)
            {
                if (i == 0) continue;
                for (var j = 0; j < word.CharacterList.Count; j++)
                {
                    if (word.Direction == Direction.Horizontal)
                    {
                        var grid = crozzle[height + i, width + j];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Vertical
                            : Direction.None;
                    }
                    else
                    {
                        var grid = crozzle[height + j, width + i];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Horizontal
                            : Direction.None;
                    }
                }
            }
        }

        private void UpdateGrid(Word word, int height, int width)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    UpdateHeadAndTailGrid(word, height, width);
                    UpdateSurroundedGrids(word, height, width);
                    break;
                case Difficulty.Medium:
                case Difficulty.Hard:
                case Difficulty.Extreme:
                    UpdateHeadAndTailGrid(word, height, width);
                    break;
            }
        }
    }
}