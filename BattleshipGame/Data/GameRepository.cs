using BattleshipGame.Data.Entities;
using BattleshipGame.Entities;

namespace BattleshipGame.Data
{
    public class GameRepository: IGameRepository
    {
        public Player _player;

        public GameRepository()
        {
            _player = new Player()
            {
                Id = 1, Name = "Player 1", Grid = new int [10, 10]
            }; 
        }

        public Player GetPlayer() 
        {
            return _player;
        }

        public void ClearGrid()
        {
            _player.Grid = new int[10, 10];
            _player.Ships = new List<Ship>();
        }

        public void AddShipsToGrid(IEnumerable<Ship> ships)
        {
            _player.Ships = ships;
        }

        public void MarkCellsOnGrid(IEnumerable<Cell> cells, int shipId)
        {
            var cellsList = cells.ToList();
            foreach (var cell in cellsList)
            {
                int cellPosX = cell.Position[0];
                int cellPosY = cell.Position[1];
                _player.Grid[cellPosY, cellPosX] = shipId;
            }
        }

        public bool IsGridCellEmpty(int[] cellPosition) 
        {
            int posX = cellPosition[0];
            int posY = cellPosition[1];

            if (_player.Grid[posY, posX] == 0)
            {
                return true;
            }

            return false;
        }
    }
}
