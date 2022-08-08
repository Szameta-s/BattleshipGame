using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;
using BattleshipGame.Data.Enums;

namespace BattleshipGame.Data
{
    public class ShipRepository : IShipRepository
    {
        private readonly IEnumerable<Ship> _ships;

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
                    int[] randStartPosition = GameUtilities.GenerateShipPosition(10);

                    if (GameUtilities.IsGridCellEmpty(newGrid, randStartPosition))
                    {
                        int nextPosX = randStartPosition[0];
                        int nextPosY = randStartPosition[1];
                        var direction = (Directions)Enum.ToObject(typeof(Directions), rand.Next(0, 4)); 

                        for (int i = 0; i < ship.Size; i++)
                        {
                            switch (direction)
                            {
                                // Up
                                case Directions.Up:
                                    nextPosY = randStartPosition[1] - i;
                                    // Making sure that cell position is not taken and in boundaries of the grid
                                    if (nextPosY >= 0 && GameUtilities.IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                // Right
                                case Directions.Right:
                                    nextPosX = randStartPosition[0] + i;
                                    if (nextPosX <= 9 && GameUtilities.IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                // Down
                                case Directions.Down:
                                    nextPosY = randStartPosition[1] + i;
                                    if (nextPosY <= 9 && GameUtilities.IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                // Left
                                case Directions.Left:
                                    nextPosX = randStartPosition[0] - i;
                                    if (nextPosX >= 0 && GameUtilities.IsGridCellEmpty(newGrid, new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY }, IsHit = false });
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (shipCells.Count == ship.Size)
                    {
                        newGrid = GameUtilities.MarkCellsOnGrid(shipCells, newGrid);
                        ship.Cells = shipCells;
                        shipCellsCreated = true;
                    }
                }   
            }

            Board board = new Board() { Size = 10, Grid = newGrid, Ships = _ships };

            return board;
        }
    }
}
