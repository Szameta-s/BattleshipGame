using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public interface IGameRepository
    {
        public Grid GetGrid();
        public bool IsGridCellEmpty(int[] cellPosition);
    }
}
