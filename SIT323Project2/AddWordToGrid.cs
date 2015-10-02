using System;
using SIT323;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{
    public class AddWordToGrid
    {
        private CrozzleGenerator _crozzleGenerator;
        private Position _position;
        private Word _word;

        public Position Position
        {
            get { return _position; }
        }

        public Word Word
        {
            get { return _word; }
        }

        public AddWordToGrid(CrozzleGenerator crozzleGenerator, Word word, int height, int wdith) :
            this(crozzleGenerator, word, new Position() { Height = height, Width = wdith })
        {
        }
        public AddWordToGrid(CrozzleGenerator crozzleGenerator, Word word,Position position)
        {
            _word = word;
            _position = position;     
            _crozzleGenerator = crozzleGenerator;
            Add();
        }

        private bool DoesWordFit()
        {
            if (Word.Direction == Direction.Horizontal)
            {
                var span = Position.Height;
                for (var i = 0; i < Word.CharacterList.Count; i++)
                {
                    var grid = _crozzleGenerator.Crozzle[span, Position.Width];
                    switch (grid.SpannableDirection)
                    {
                        case Direction.Vertical:
                        case Direction.All:
                            return false;
                    }
                    if (grid.Character != '\0' && grid.Character != Word.CharacterList[i].Alphabetic)
                    {
                        return false;
                    }
                    span++;
                }
            }
            else
            {
                var span = Position.Width;
                for (var i = 0; i < Word.CharacterList.Count; i++)
                {
                    var grid = _crozzleGenerator.Crozzle[Position.Height, Position.Width];
                    switch (grid.SpannableDirection)
                    {
                        case Direction.Horizontal:
                        case Direction.All:
                            return false;
                    }
                    if (grid.Character != '\0' && grid.Character != Word.CharacterList[i].Alphabetic)
                    {
                        return false;
                    }
                    span++;
                }
            }
            return true;
        }

        private void Add()
        {
            _crozzleGenerator.Crozzle.Wordlist.Add(Word);
            if (Word.Direction == Direction.Vertical)
            {
                var span = Position.Height;
                for (var i = 0; i < Word.CharacterList.Count; i++)
                {
                    var grid = _crozzleGenerator.Crozzle[span, Position.Width];
                    grid.Character = Word.CharacterList[i].Alphabetic;
                    Word.CharacterList[i].Position = new Position { Height = span, Width = Position.Width };
                    grid.VerticalWord = Word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Horizontal
                        : Direction.None;
                    span++;
                }
            }
            else
            {
                var span = Position.Width;
                for (var i = 0; i < Word.CharacterList.Count; i++)
                {
                    var grid = _crozzleGenerator.Crozzle[Position.Height, span];
                    grid.Character = Word.CharacterList[i].Alphabetic;
                    Word.CharacterList[i].Position = new Position { Height = Position.Height, Width = span };
                    grid.HorizontalWord = Word;
                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Vertical
                        : Direction.None;
                    span++;
                }
            }
            UpdateGrid();
        }

        /// <summary>
        ///     EEEEEEEE
        ///     EXWORDXE
        ///     EEEEEEEE
        ///     E = Empty
        ///     X = To be Touched
        /// </summary>
        private void UpdateHeadAndTailGrid()
        {
            Grid headGrid;
            Grid tailGrid;
            if (Word.Direction == Direction.Vertical)
            {
                headGrid = _crozzleGenerator.Crozzle[Position.Height - 1, Position.Width];
                tailGrid = _crozzleGenerator.Crozzle[Position.Height + Word.CharacterList.Count, Position.Width];
            }
            else
            {
                headGrid = _crozzleGenerator.Crozzle[Position.Height, Position.Width - 1];
                tailGrid = _crozzleGenerator.Crozzle[Position.Height, Position.Width + Word.CharacterList.Count];
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
        private void UpdateSurroundedGrids()
        {
            for (var i = -1; i < 2; i++)
            {
                if (i == 0) continue;
                for (var j = 0; j < Word.CharacterList.Count; j++)
                {
                    if (Word.Direction == Direction.Horizontal)
                    {
                        var grid = _crozzleGenerator.Crozzle[Position.Height + i, Position.Width + j];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Vertical
                            : Direction.None;
                    }
                    else
                    {
                        var grid = _crozzleGenerator.Crozzle[Position.Height + j, Position.Width + i];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                            ? Direction.Horizontal
                            : Direction.None;
                    }
                }
            }
        }

        private void UpdateGrid()
        {
            switch (_crozzleGenerator.Wordlist.Level)
            {
                case Difficulty.Easy:
                    UpdateHeadAndTailGrid();
                    UpdateSurroundedGrids();
                    break;
                case Difficulty.Medium:
                case Difficulty.Hard:
                case Difficulty.Extreme:
                    UpdateHeadAndTailGrid();
                    break;
            }
        }
    }
}