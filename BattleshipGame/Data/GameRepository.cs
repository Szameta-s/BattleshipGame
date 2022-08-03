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

        public bool IsGridCellEmpty(int[] cellPosition) 
        {
            int posX = cellPosition[0];
            int posY = cellPosition[1];

            if (_grid.Board[posY, posX] == 0)
            {
                _grid.Board[posY, posX] = 1;
                return true;
            }

            return false;
        }
    }
}
