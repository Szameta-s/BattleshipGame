using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data.Models
{
    public class ShotDataModel
    {
        public Ship Ship { get; set; }
        public Cell Cell { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}
