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
        private Character _character;
        private List<Character> _preCharacterPlaceable;
        private List<Character> _postCharacterPlaceable;
        private Position _position;



        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Character Character
        {
            get { return _character; }
            set { _character = value; }
        }

        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public List<Character> PreCharacterPlaceable
        {
            get { return _preCharacterPlaceable; }
            set { _preCharacterPlaceable = value; }
        }

        public List<Character> PostCharacterPlaceable
        {
            get { return _postCharacterPlaceable; }
            set { _postCharacterPlaceable = value; }
        }
    }
}
