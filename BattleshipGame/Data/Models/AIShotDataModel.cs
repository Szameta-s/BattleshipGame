using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data.Models
{
    public class AIShotDataModel
    {
        public Board Board { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
    }
}
