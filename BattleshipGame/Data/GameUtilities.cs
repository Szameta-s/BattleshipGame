using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public static class GameUtilities
    {
        public static Ship MarkShipCellWithHit(Ship ship, Cell cell)
        {
            int xPos = cell.Position[0];
            int yPos = cell.Position[1];

            var shipCell = ship.Cells.Where(c => c.Position[0] == xPos && c.Position[1] == yPos).FirstOrDefault();

            shipCell.IsHit = true;
            ship.Hitpoints--;

            return ship;
        }
    
        public static IEnumerable<Cell> GenerateShot(IEnumerable<Cell> cells)
        {
            bool foundNextshot = false;
            var cellsList = cells.ToList();

            while (!foundNextshot)
            {
                int[] position = GenerateShipPosition(10);
                int xPos = position[0];
                int yPos = position[1];

                if ((!cells.Any(c => c.Position[0] == xPos && c.Position[1] == yPos)) || (!cells.Any()))
                {
                    Cell targetCell = new Cell() { Position = new int[] { xPos, yPos }, IsHit = false };
                    cellsList.Add(targetCell);
                    foundNextshot = true;
                }
            }

            return cellsList;
        }

        public static bool IsCellDuplicate(IEnumerable<Cell> cells, Cell cell)
        {
            int xPos = cell.Position[0];
            int yPos = cell.Position[1];

            if ((!cells.Any(c => c.Position[0] == xPos && c.Position[1] == yPos)) || (!cells.Any()))
            {
                return false;
            }

            return true;
        }

        public static int[,] MarkCellsOnGrid(IEnumerable<Cell> cells, int[,] grid)
        {
            var cellsList = cells.ToList();
            foreach (var cell in cellsList)
            {
                int cellPosX = cell.Position[0];
                int cellPosY = cell.Position[1];
                grid[cellPosY, cellPosX] = cell.ShipId;
            }

            return grid;
        }

        public static bool IsGridCellEmpty(int[,] grid, int[] cellPosition)
        {
            int posX = cellPosition[0];
            int posY = cellPosition[1];

            if (grid[posY, posX] == 0)
            {
                return true;
            }

            return false;
        }

        public static int[] GenerateShipPosition(int range)
        {
            Random rand = new Random();
            int randPosX = rand.Next(0, range);
            int randPosY = rand.Next(0, range);

            return new int[2] { randPosX, randPosY };
        }
    }
}
