using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public interface IShipRepository
    {
        public IEnumerable<Ship> GetAllShips();
        public Ship GetShipById(int id);
        public Board GenerateBoard();
    }
}
