using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrozzleApplication
{
    class WordData
    {
        public String letters;
        public Coordinate location;
        public Boolean horizontal;

        #region constructors
        public WordData(String sequence, int row, int column, Boolean direction)
        {
            letters = sequence;
            location = new Coordinate(row, column);
            horizontal = direction;
        }
        #endregion
    }
}