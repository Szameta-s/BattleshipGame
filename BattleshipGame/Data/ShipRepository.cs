using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class ShipRepository : IShipRepository
    {
        IEnumerable<Ship> _ships;
        IGameRepository _gameRepository;

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

        public IEnumerable<Ship> GenerateShipCells() 
        {
            Random rand = new Random();
            foreach (Ship ship in _ships)
            {
                List<Cell> shipCells = new List<Cell>();
                bool shipCellsCreated = false;
                
                while (!shipCellsCreated)
                {
                    int[] randStartPosition = GenerateShipPosition(9);

                    if (_gameRepository.IsGridCellEmpty(randStartPosition))
                    {
                        int nextPosX = randStartPosition[0];
                        int nextPosY = randStartPosition[1];
                        shipCells.Add(new Cell() { Position = new[] { nextPosX, nextPosY } });

                        int direction = rand.Next(0, 3);
                        for (int i = 1; i < ship.Size; i++)
                        {
                            switch (direction)
                            {
                                case 0:
                                    nextPosY = randStartPosition[1] - i;
                                    if (nextPosY >= 0 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { Position = new[] { nextPosX, nextPosY } });
                                    break;
                                case 1:
                                    nextPosX = randStartPosition[0] + i;
                                    if (nextPosX <= 9 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { Position = new[] { nextPosX, nextPosY } });
                                    break;
                                case 2:
                                    nextPosY = randStartPosition[1] + i;
                                    if (nextPosY <= 9 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { Position = new[] { nextPosX, nextPosY } });
                                    break;
                                case 3:
                                    nextPosX = randStartPosition[0] - i;
                                    if (nextPosX >= 0 && _gameRepository.IsGridCellEmpty(new[] { nextPosX, nextPosY }))
                                        shipCells.Add(new Cell() { Position = new[] { nextPosX, nextPosY } });
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (shipCells.Count() == ship.Size)
                    {
                        ship.Cells = shipCells;
                        shipCellsCreated = true;
                    }
                }          
            }
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
