using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrozzleApplication
{
    class CrozzleMap
    {
        const int extraRows = 2;
        const int extraColumns = 2;

        private Boolean[,] map;

        #region constructors
        public CrozzleMap(List<String[]> crozzleRows, List<String[]> crozzleColumns)
        {
            // Create a 2D array of Boolean that is initialised to false.
            // For coding "neatness", it has an extra row at the top and one at the bottom, and
            // an extra column on the left and one on the right
            int numberOfRows = crozzleRows.Count + extraRows;
            int numberOfColumns = crozzleColumns.Count + extraColumns;
            map = new Boolean[numberOfRows, numberOfColumns];

            // Store a true value in the map at the same location as each letter
            this.mapLetters(crozzleRows);
        }
        #endregion

        #region properties
        public Boolean ContainsGroup
        {
            get
            {
                Boolean found = false;

                foreach (Boolean status in map)
                {
                    if (status)
                    {
                        found = true;
                        break;
                    }
                }
                return (found);
            }
        }
        #endregion

        #region create the Boolean map
        private void mapLetters(List<String[]> crozzleRows)
        {
            int row;
            int column;

            row = 0;
            foreach (String[] letters in crozzleRows)
            {
                row++;
                column = 0;
                foreach (String letter in letters)
                {
                    column++;
                    if (letter[0] != ' ')
                    {
                        map[row, column] = true;
                    }
                }
            }
        }
        #endregion

        #region remove a group
        public void RemoveGroup()
        {
            // Remove a group of words from the map. If all words are connected as one group, 
            // the map ends up containing only false values

            // the start position can be the location of any letter
            Coordinate start = this.FindLocation();

            // the recursive call needs a List of Coordinates
            List<Coordinate> locations = new List<Coordinate>();
            locations.Add(start);

            // remove a group
            RemoveGroup(locations);
        }

        public Coordinate FindLocation()
        {
            Coordinate location = new Coordinate(-1, -1);

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int column = 0; column < map.GetLength(1); column++)
                {
                    if (map[row, column])
                    {
                        location.row = row;
                        location.column = column;
                    }
                }
            }
            return (location);
        }

        private void RemoveGroup(List<Coordinate> locations)
        {
            // Remove a group of words from the map. If all words are connected as one group, 
            // the map ends up containing only false values

            foreach (Coordinate location in locations)
            {
                // remove letter indicator from map
                map[location.row, location.column] = false;

                // get the locations of letters that are "next" to the current letter
                List<Coordinate> adjacentLocations = GetAdjacentLocations(location);

                // recursively remove more of the group of words
                RemoveGroup(adjacentLocations);
            }
        }

        private List<Coordinate> GetAdjacentLocations(Coordinate location)
        {
            List<Coordinate> adjacentLocations = new List<Coordinate>();

            if (map[location.row, location.column - 1] == true)
            {
                Coordinate loc = new Coordinate(location.row, location.column - 1);
                adjacentLocations.Add(loc);
            }

            if (map[location.row, location.column + 1] == true)
            {
                Coordinate loc = new Coordinate(location.row, location.column + 1);
                adjacentLocations.Add(loc);
            }

            if (map[location.row - 1, location.column] == true)
            {
                Coordinate loc = new Coordinate(location.row - 1, location.column);
                adjacentLocations.Add(loc);
            }

            if (map[location.row + 1, location.column] == true)
            {
                Coordinate loc = new Coordinate(location.row + 1, location.column);
                adjacentLocations.Add(loc);
            }

            return (adjacentLocations);
        }
        #endregion
    }
}