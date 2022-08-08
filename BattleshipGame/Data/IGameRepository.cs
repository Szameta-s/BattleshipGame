using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public interface IGameRepository
    {
        public IEnumerable<Player> GetPlayers();
        public Player GetPlayerById(int id);
    }
}
