using System;
using System.Runtime.InteropServices;
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
        private bool _added;

        public bool Added
        {
            get { return _added; }
        }

        public Position Position
        {
            get { return _position; }
        }

        public Word Word
        {
            get { return _word; }
        }

        public AddWordToGrid(CrozzleGenerator crozzleGenerator, Word word, int height, int wdith) :
            this(crozzleGenerator, word, new Position() {Height = height, Width = wdith})
        {
        }

        public AddWordToGrid(CrozzleGenerator crozzleGenerator, Word word, Position position)
        {
            _word = word;
            _position = position;
            _crozzleGenerator = crozzleGenerator;
            _added = false;

            if (CheckHeadTail())
            {
                
                    if (Add()) _added = true;
                
            }
            else
            {
                var tt = 0;
            }
        }



        private bool CheckHeadTail()
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
            if (headGrid != null && !headGrid.IsCharacterNullOrSpaced()) return false;
            if (tailGrid != null && !tailGrid.IsCharacterNullOrSpaced()) return false;
            return true;
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
                    if (!grid.IsCharacterNullOrSpaced() && grid.Character.Alphabetic != Word.CharacterList[i].Alphabetic)
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
                    if (grid.Character.Alphabetic != default(char) && grid.Character.Alphabetic != Word.CharacterList[i].Alphabetic)
                    {
                        return false;
                    }
                    span++;
                }
            }
            return true;
        }

        private bool Add()
        {
            if (Word.Direction == Direction.Vertical)
            {
                var span = Position.Height;
                for (var i = 0; i < Word.CharacterList.Count; i++)
                {
                    var grid = _crozzleGenerator.Crozzle[span, Position.Width];
                    //if (grid == null) return false;
                    grid.Character = Word.CharacterList[i];
                    Word.CharacterList[i].Position = new Position {Height = span, Width = Position.Width};
                    grid.VerticalWord = Word;

                    if (grid.HorizontalWord != null)
                    {
                        grid.HorizontalWord.IntersectWords.Add(Word);
                        Word.IntersectWords.Add(grid.HorizontalWord);
                    }


                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Horizontal
                        : Direction.None;

//                    if (grid.HorizontalWord == null && grid.SpannableDirection == Direction.Vertical)
//                    {
//                        grid.SpannableDirection = Direction.Horizontal;
//                    }
//                    else
//                    {
//                       grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
//                        ? Direction.Horizontal
//                        : Direction.None;
//                    
//                    }
                    span++;
                }
            }
            else
            {
                var span = Position.Width;
                for (var i = 0; i < Word.CharacterList.Count; i++)
                {
                    var grid = _crozzleGenerator.Crozzle[Position.Height, span];
                    //if (grid == null) return false;

                    grid.Character = Word.CharacterList[i];
                    Word.CharacterList[i].Position = new Position {Height = Position.Height, Width = span};
                    grid.HorizontalWord = Word;

                    if (grid.VerticalWord != null)
                    {
                        grid.VerticalWord.IntersectWords.Add(Word);
                        Word.IntersectWords.Add(grid.VerticalWord);
                    }

                    grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
                        ? Direction.Vertical
                        : Direction.None;

//                    if (grid.VerticalWord == null && grid.SpannableDirection == Direction.Horizontal)
//                    {
//                        grid.SpannableDirection = Direction.Vertical;
//                    }
//                    else
//                    {
//                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All)
//                            ? Direction.Vertical
//                            : Direction.None;
//                    }
                    span++;
                }
            }
            _crozzleGenerator.Crozzle.Wordlist.Add(Word);
            UpdateGrid();
            return true;
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
            if (Word.ToString() == "DASH")
            {
                int tt = 0;
            }
            for (var i = -1; i < 2; i++)
            {
                if (i == 0) continue;
                for (var j = 0; j < Word.CharacterList.Count; j++)
                {
                    if (Word.Direction == Direction.Horizontal)
                    {
                        var grid = _crozzleGenerator.Crozzle[Position.Height + i, Position.Width + j];
                        if (grid == null) continue;

                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All ||
                                                   grid.SpannableDirection == Direction.Vertical)
                            ? Direction.Vertical
                            : Direction.None;

//                        if (grid.VerticalWord != null && 
//                            grid.HorizontalWord == null &&
//                            grid.SpannableDirection == Direction.Horizontal
//                            )
//                        {
//                            grid.SpannableDirection = Direction.Horizontal;
//                        }
//                        else
//                        {
//                            grid.SpannableDirection = (grid.SpannableDirection == Direction.All ||
//                                                       grid.SpannableDirection == Direction.Vertical)
//                                ? Direction.Vertical
//                                : Direction.None;
//                        }
                    }
                    else
                    {
                        var grid = _crozzleGenerator.Crozzle[Position.Height + j, Position.Width + i];
                        if (grid == null) continue;
                        grid.SpannableDirection = (grid.SpannableDirection == Direction.All ||
                                                   grid.SpannableDirection == Direction.Horizontal)
                            ? Direction.Horizontal
                            : Direction.None;
//
//                        if (grid.HorizontalWord != null &&
//                            grid.VerticalWord == null &&
//                            grid.SpannableDirection == Direction.Vertical
//                            )
//                        {
//                            grid.SpannableDirection = Direction.Vertical;
//                        }
//                        else
//                        {
//                            grid.SpannableDirection = (grid.SpannableDirection == Direction.All ||
//                                                       grid.SpannableDirection == Direction.Horizontal)
//                                ? Direction.Horizontal
//                                : Direction.None;
//                        }
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
                    break;
                case Difficulty.Medium:
                case Difficulty.Hard:
                case Difficulty.Extreme:
                    UpdateSurroundedGrids();
                    UpdateHeadAndTailGrid();
                    break;
            }
        }
    }
}