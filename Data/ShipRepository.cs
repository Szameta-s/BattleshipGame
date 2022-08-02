using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class ShipRepository : IShipRepository
    {
        IEnumerable<Ship> _ships;

        public ShipRepository()
        {
            _ships = new List<Ship>()
            {
                new Ship() { Id = 1, Name = "Destroyer", Size = 2},
                new Ship() { Id = 2, Name = "Cruiser", Size = 3},
                new Ship() { Id = 3, Name = "Submarine", Size = 3},
                new Ship() { Id = 4, Name = "Battleship", Size = 4},
                new Ship() { Id = 5, Name = "Carrier", Size = 5}
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
    }
}
