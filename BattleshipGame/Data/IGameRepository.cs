using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public interface IGameRepository
    {
        public Player GetPlayer();
        public void ClearGrid();
        public void AddShipsToGrid(IEnumerable<Ship> ships);
        public void MarkCellsOnGrid(IEnumerable<Cell> cells, int shipId);
        public bool IsGridCellEmpty(int[] cellPosition);
    }
}
