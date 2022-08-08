using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class ShipRepository : IShipRepository
    {
        public IEnumerable<Ship> _ships;

        public ShipRepository()
        {   
            _ships = new List<Ship>()
            {
                new Ship() { Id = 1, Name = "Destroyer", Size = 2, Hitpoints = 2},
                new Ship() { Id = 2, Name = "Cruiser", Size = 3, Hitpoints = 3},
                new Ship() { Id = 3, Name = "Submarine", Size = 3, Hitpoints = 3},
                new Ship() { Id = 4, Name = "Battleship", Size = 4, Hitpoints = 4},
                new Ship() { Id = 5, Name = "Carrier", Size = 5, Hitpoints = 5}
            };
        }
        public IEnumerable<Ship> GetAllShips()
        {
            return _ships;
        }

        public Ship GetShipById(int id)
        {
            Ship ship = new Ship();

            try
            {
                ship = _ships.Single(ship => ship.Id == id);
            }
            catch (Exception ex) 
            {
            }

            return ship;
        }

        public Ship MarkShipCellWithHit(Ship ship, Cell cell)
        {
            int xPos = cell.Position[0];
            int yPos = cell.Position[1];

            var shipCell = ship.Cells.Where(c => c.Position[0] == xPos && c.Position[1] == yPos).FirstOrDefault();

            if (shipCell.IsHit == false)
            {
                shipCell.IsHit = true;
                ship.Hitpoints--;
            }         

            return ship;
        }

        public IEnumerable<Cell> GenerateShot(IEnumerable<Cell> cells) 
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

        public bool IsCellDuplicate(IEnumerable<Cell> cells, Cell cell) 
        {
            int xPos = cell.Position[0];
            int yPos = cell.Position[1];

            if ((!cells.Any(c => c.Position[0] == xPos && c.Position[1] == yPos)) || (!cells.Any()))
            {
                return false;
            }

            return true;
        }

        public Board GenerateBoard() 
        {
            Random rand = new Random();
            int[,] newGrid = new int[10, 10];

            foreach (Ship ship in _ships)
            {   
                bool shipCellsCreated = false;
                
                while (!shipCellsCreated)
                {
                    List<Cell> shipCells = new List<Cell>();
                    int[] randStartPosition = GenerateShipPosition(10);

                    if (IsGridCellEmpty(newGrid, randStartPosition))
                    {
                        int nextPosX = randStartPosition[0];
                        int nextPosY = randStartPosition[1];
                        int direction = rand.Next(0, 4);

                        for (int i = 0; i < ship.Size; i++)
                        {
                            switch (direction)
                            {
                                // Up
                                case 0:
                                    nextPosY = randStartPosition[1] - i;
                                    // Making sure that cell position is not taken and in boundaries of the grid
                                    if (nextPosY >= 0 && IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                // Right
                                case 1:
                                    nextPosX = randStartPosition[0] + i;
                                    if (nextPosX <= 9 && IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                // Down
                                case 2:
                                    nextPosY = randStartPosition[1] + i;
                                    if (nextPosY <= 9 && IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                // Left
                                case 3:
                                    nextPosX = randStartPosition[0] - i;
                                    if (nextPosX >= 0 && IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (shipCells.Count == ship.Size)
                    {
                        newGrid = MarkCellsOnGrid(shipCells, newGrid);
                        ship.Cells = shipCells;
                        shipCellsCreated = true;
                    }
                }   
            }

            Board board = new Board() { Size = 10, Grid = newGrid, Ships = _ships };

            return board;
        }

        public int[,] MarkCellsOnGrid(IEnumerable<Cell> cells, int[,] grid)
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

        public bool IsGridCellEmpty(int[,] grid, int[] cellPosition)
        {
            int posX = cellPosition[0];
            int posY = cellPosition[1];

            if (grid[posY, posX] == 0)
            {
                return true;
            }

            return false;
        }

        public int[] GenerateShipPosition(int range)
        {
            Random rand = new Random();
            int randPosX = rand.Next(0, range);
            int randPosY = rand.Next(0, range);

            return new int[2] { randPosX, randPosY };
        }
    }
}
