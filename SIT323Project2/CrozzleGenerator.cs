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
        private readonly CrozzleProject2 crozzle;

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

        public void PlaceSingleWordToGrid(Word word, int width, int height)
        {
            AddWordToGrid(word, width, height);
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
            PlaceSingleWordToGrid(word, pos.Width, pos.Height);
        }

        public void PlaceWordsToGrid()
        {
            Word word;
            if (crozzle.Wordlist.Count == 0)
            {
                word = new Word(Direction.Vertical, WordsNotAddedList.FirstOrDefault());
                PlaceSingleWordToGrid(word, 0, 0);
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

        private bool DoesWordFit(Word word, int width, int height)
        {
            if (word.Direction == Direction.Vertical)
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
                    var grid = crozzle[span, width];
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


        public void AddWordToGrid(Word word, int width, int height)
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
                    grid.HorizontalWord = word;
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
                    var grid = crozzle[height, span];
                    grid.Character = word.CharacterList[i].Alphabetic;
                    word.CharacterList[i].Position = new Position {Height = height, Width = span};
                    grid.HorizontalWord = word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Horizontal
                        : Direction.None;
                    span++;
                }
            }
            UpdateGrid(word, width, height);
        }

        /// <summary>
        ///     EEEEEEEE
        ///     EXWORDXE
        ///     EEEEEEEE
        ///     E = Empty
        ///     X = To be Touched
        /// </summary>
        private void UpdateHeadAndTailGrid(Word word, int width, int height)
        {
            Grid headGrid;
            Grid tailGrid;
            if (word.Direction == Direction.Horizontal)
            {
                headGrid = crozzle[width, height - 1];
                tailGrid = crozzle[width, height + word.CharacterList.Count];
            }
            else
            {
                headGrid = crozzle[width - 1, height];
                tailGrid = crozzle[width + word.CharacterList.Count, height];
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
        private void UpdateSurroundedGrids(Word word, int width, int height)
        {
            for (var i = -1; i < 2; i++)
            {
                if (i == 0) continue;
                for (var j = 0; j < word.CharacterList.Count; j++)
                {
                    if (word.Direction == Direction.Horizontal)
                    {
                        var grid = crozzle[width + i, height + j];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Vertical
                            : Direction.None;
                    }
                    else
                    {
                        var grid = crozzle[width + j, height + i];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Horizontal
                            : Direction.None;
                    }
                }
            }
        }

        private void UpdateGrid(Word word, int width, int height)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    UpdateHeadAndTailGrid(word, width, height);
                    UpdateSurroundedGrids(word, width, height);
                    break;
                case Difficulty.Medium:
                case Difficulty.Hard:
                case Difficulty.Extreme:
                    UpdateHeadAndTailGrid(word, width, height);
                    break;
            }
        }
    }
}