using BattleshipGame.Entities;

namespace BattleshipGame.Data.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Board Board { get; set; }
    }
}
