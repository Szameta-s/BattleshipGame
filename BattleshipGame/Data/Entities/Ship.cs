using BattleshipGame.Data.Entities;

namespace BattleshipGame.Entities
{
    public class Ship
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Size { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}
