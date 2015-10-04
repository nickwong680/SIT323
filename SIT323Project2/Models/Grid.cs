using SIT323.Models;

namespace SIT323Project2.Models
{
    public class Grid
    {
        public Grid()
        {
            Character = new Character(default(char));
            SpannableDirection = Direction.All;
        }

        public Position Position { get; set; }

        public Direction SpannableDirection { get; set; }

        public Word HorizontalWord { get; set; }

        public Word VerticalWord { get; set; }

        public Character Character { get; set; }

        public bool IsCharacterNullOrSpaced()
        {
            if (Character != null)
            {
                if (Character.Alphabetic != default(char))
                {
                    return false;
                }
            }
            return true;
        }
    }
}