using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIT323.Models;

namespace SIT323Project2.Models
{
    public class Grid
    {
        private Direction _spannableDirection;
        private Word _horizontalWord;
        private Word _verticalWord;
        private char _character;
        private Position _position;

        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Direction SpannableDirection
        {
            get { return _spannableDirection; }
            set { _spannableDirection = value; }
        }

        public Word HorizontalWord
        {
            get { return _horizontalWord; }
            set { _horizontalWord = value; }
        }

        public Word VerticalWord
        {
            get { return _verticalWord; }
            set { _verticalWord = value; }
        }

        public char Character
        {
            get { return _character; }
            set { _character = value; }
        }

        public Grid()
        {
            _spannableDirection = Direction.All;
        }
    }
}
