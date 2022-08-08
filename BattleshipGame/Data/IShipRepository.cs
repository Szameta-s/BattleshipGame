using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public interface IShipRepository
    {
        public IEnumerable<Ship> GetAllShips();
        public Ship GetShipById(int id);
        public Ship MarkShipCellWithHit(Ship ship, Cell cell);
        public IEnumerable<Cell> GenerateShot(IEnumerable<Cell> cells);
        public bool IsCellDuplicate(IEnumerable<Cell> cells, Cell cell);
        public Board GenerateBoard();
        public int[,] MarkCellsOnGrid(IEnumerable<Cell> cells, int[,] grid);
        public bool IsGridCellEmpty(int[,] grid, int[] cellPosition);
        public int[] GenerateShipPosition(int range);
    }
}
