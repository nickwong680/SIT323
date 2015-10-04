using SIT323;
using SIT323.Models;
using SIT323Project2.Models;

namespace SIT323Project2
{
    public class AddWordToGrid
    {
        private readonly CrozzleGenerator _crozzleGenerator;

        public AddWordToGrid(CrozzleGenerator crozzleGenerator, Word word, int height, int wdith) :
            this(crozzleGenerator, word, new Position {Height = height, Width = wdith})
        {
        }

        public AddWordToGrid(CrozzleGenerator crozzleGenerator, Word word, Position position)
        {
            Word = word;
            Position = position;
            _crozzleGenerator = crozzleGenerator;
            Added = false;

            if (CheckHeadTail())
            {
                if (Add()) Added = true;
            }
        }

        public bool Added { get; private set; }

        public Position Position { get; private set; }

        public Word Word { get; private set; }


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
        ///     EEXXXXEE
        ///     EEWORDEE
        ///     EEXXXXEE
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
                    UpdateSurroundedGrids();
                    UpdateHeadAndTailGrid();
                    UpdateLeftAndRightBorder();

                    break;
                case Difficulty.Medium:
                case Difficulty.Hard:
                case Difficulty.Extreme:
                    UpdateSurroundedGrids();
                    UpdateHeadAndTailGrid();
                    break;
            }
        }

        /// <summary>
        ///     EXEEEEXE
        ///     EEWORDEE
        ///     EXEEEEXE
        ///     E = Empty
        ///     X = To be Touched
        /// </summary>
        private void UpdateLeftAndRightBorder()
        {
            for (var i = -1; i < 2; i++)
            {
                if (i == 0) continue;

                Grid headGrid;
                Grid tailGrid;

                if (Word.Direction == Direction.Vertical)
                {
                    headGrid = _crozzleGenerator.Crozzle[Position.Height - 1, Position.Width + i];
                    tailGrid = _crozzleGenerator.Crozzle[Position.Height + Word.CharacterList.Count, Position.Width + i];
                }
                else
                {
                    headGrid = _crozzleGenerator.Crozzle[Position.Height + i, Position.Width - 1];
                    tailGrid = _crozzleGenerator.Crozzle[Position.Height + i, Position.Width + Word.CharacterList.Count];
                }
                if (Word.Direction == Direction.Vertical)
                {
                    if (headGrid != null)
                        headGrid.SpannableDirection = (headGrid.SpannableDirection == Direction.All ||
                                                       headGrid.SpannableDirection == Direction.Horizontal)
                            ? Direction.Horizontal
                            : Direction.None;

                    if (tailGrid != null)
                        tailGrid.SpannableDirection = (tailGrid.SpannableDirection == Direction.All ||
                                                       tailGrid.SpannableDirection == Direction.Horizontal)
                            ? Direction.Horizontal
                            : Direction.None;
                }
                else
                {
                    if (headGrid != null)
                        headGrid.SpannableDirection = (headGrid.SpannableDirection == Direction.All ||
                                                       headGrid.SpannableDirection == Direction.Vertical)
                            ? Direction.Vertical
                            : Direction.None;

                    if (tailGrid != null)
                        tailGrid.SpannableDirection = (tailGrid.SpannableDirection == Direction.All ||
                                                       tailGrid.SpannableDirection == Direction.Vertical)
                            ? Direction.Vertical
                            : Direction.None;
                }
            }
        }
    }
}