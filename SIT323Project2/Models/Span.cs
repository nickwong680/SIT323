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
        private Direction direction;
        private char character;
        private List<char> preCharacterPlaceable;
        private List<char> postCharacterPlaceable;
        private Position position;


        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public char Character
        {
            get { return character; }
            set { character = value; }
        }

        public Position Position
        {
            get { return position; }
            set { position = value; }
        }

        public List<char> PreCharacterPlaceable
        {
            get { return preCharacterPlaceable; }
            set { preCharacterPlaceable = value; }
        }

        public List<char> PostCharacterPlaceable
        {
            get { return postCharacterPlaceable; }
            set { postCharacterPlaceable = value; }
        }
    }
}
