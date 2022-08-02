namespace BattleshipGame.Entities
{
    public class Grid
    {
        public int PlayerId { get; set; }
        public int Size { get; set; }
        public int[,] Board { get; set; }
        public IEnumerable<Ship> Ships { get; set; }
    }
}
