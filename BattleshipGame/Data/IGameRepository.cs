using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public interface IGameRepository
    {
        public Grid GetGrid();
        public void ClearGrid();
        public void AddShipsToGrid(IEnumerable<Ship> ships);
        public void MarkCellsOnGrid(IEnumerable<Cell> cells);
        public bool IsGridCellEmpty(int[] cellPosition);
    }
}
