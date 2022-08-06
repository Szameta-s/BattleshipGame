using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class ShipRepository : IShipRepository
    {
        public IEnumerable<Ship> _ships;
        private readonly IGameRepository _gameRepository;

        public ShipRepository(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;

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
            try
            {
                Ship ship = _ships.Single(ship => ship.Id == id);
                return ship;
            }
            catch (Exception ex) 
            {
                return null;
            }         
        }

        public Ship MarkShipCellWithHit(Cell cell)
        {
            int xPos = cell.Position[0];
            int yPos = cell.Position[1];

            var targetShip = _ships.Where(ship => ship.Id == cell.ShipId).FirstOrDefault();
            var targetCell = targetShip.Cells.Where(shipCell => shipCell.Position[0] == xPos && shipCell.Position[1] == yPos).FirstOrDefault();

            if (targetCell.IsHit == false)
            {
                targetCell.IsHit = true;
                targetShip.Hitpoints--;
            }         

            return targetShip;
        }

        public IEnumerable<Ship> GenerateShipCells() 
        {
            Random rand = new Random();
            _gameRepository.ClearGrid();

            foreach (Ship ship in _ships)
            {   
                bool shipCellsCreated = false;
                
                while (!shipCellsCreated)
                {
                    List<Cell> shipCells = new List<Cell>();
                    int[] randStartPosition = GenerateShipPosition(10);

                    if (_gameRepository.IsGridCellEmpty(randStartPosition))
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
                                    // Check if cell posision is in boundaries of the grid
                                    if (nextPosY >= 0 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY } });
                                    break;
                                // Right
                                case 1:
                                    nextPosX = randStartPosition[0] + i;
                                    if (nextPosX <= 9 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY } });
                                    break;
                                // Down
                                case 2:
                                    nextPosY = randStartPosition[1] + i;
                                    if (nextPosY <= 9 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY } });
                                    break;
                                // Left
                                case 3:
                                    nextPosX = randStartPosition[0] - i;
                                    if (nextPosX >= 0 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { ShipId = ship.Id, Position = new[] { nextPosX, nextPosY } });
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (shipCells.Count() == ship.Size)
                    {
                        _gameRepository.MarkCellsOnGrid(shipCells, ship.Id);
                        ship.Hitpoints = ship.Size;
                        ship.Cells = shipCells;
                        shipCellsCreated = true;
                    }
                }          
            }
            _gameRepository.AddShipsToGrid(_ships);
            return _ships;
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
