namespace BattleshipGame.Entities
{
    public class Board
    {
        public int Size { get; set; }
        public int[,] Grid { get; set; }
        public IEnumerable<Ship> Ships { get; set; }
    }
}
