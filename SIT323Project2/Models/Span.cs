using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIT323.Models;

namespace SIT323Project2.Models
{
    public class Span
    {
        private Direction _direction;
        private char _character;
        private List<char> _preCharacterPlaceable;
        private List<char> _postCharacterPlaceable;
        private Position _position;



        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public char Character
        {
            get { return _character; }
            set { _character = value; }
        }

        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public List<char> PreCharacterPlaceable
        {
            get { return _preCharacterPlaceable; }
            set { _preCharacterPlaceable = value; }
        }

        public List<char> PostCharacterPlaceable
        {
            get { return _postCharacterPlaceable; }
            set { _postCharacterPlaceable = value; }
        }
    }
}
