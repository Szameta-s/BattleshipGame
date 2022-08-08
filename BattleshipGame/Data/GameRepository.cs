using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class GameRepository: IGameRepository
    {
        public IEnumerable<Player> _players;

        public GameRepository()
        {
            _players = new List<Player>()
            {
                new Player() { Id = 1, Name = "Player 1", Board = new Board() },
                new Player() { Id = 2, Name = "Player 2", Board = new Board() }
            };
        }

        public IEnumerable<Player> GetPlayers() 
        {
            return _players;
        }

        public Player GetPlayerById(int id)
        {
            try
            {
                Player player = _players.Single(p => p.Id == id);
                return player;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
