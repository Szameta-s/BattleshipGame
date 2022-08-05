using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class GameRepository: IGameRepository
    {
        Grid _grid;

        public GameRepository()
        {
            _grid = new Grid()
            {
                PlayerId = 1, Size = 10, Board = new int[10, 10] 
            };
        }

        public Grid GetGrid()
        {
            return _grid;
        }

        public void ClearGrid()
        {
            _grid = new Grid()
            {
                PlayerId = 1,
                Size = 10,
                Board = new int[10, 10],
                Ships = new List<Ship>()
            };
        }

        public void AddShipsToGrid(IEnumerable<Ship> ships)
        {
            _grid.Ships = ships;
        }

        public void MarkCellsOnGrid(IEnumerable<Cell> cells)
        {
            var cellsList = cells.ToList();
            foreach (var cell in cellsList)
            {
                int cellPosX = cell.Position[0];
                int cellPosY = cell.Position[1];
                _grid.Board[cellPosY, cellPosX] = 1;
            }
        }

        public bool IsGridCellEmpty(int[] cellPosition) 
        {
            int posX = cellPosition[0];
            int posY = cellPosition[1];

            if (_grid.Board[posY, posX] == 0)
            {
                return true;
            }

            return false;
        }
    }
}
