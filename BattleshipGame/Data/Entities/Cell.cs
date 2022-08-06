namespace BattleshipGame.Data.Entities
{
    public class Cell
    {
        public int ShipId { get; set; }
        public int[] Position { get; set; }
        public bool IsHit { get; set; }
    }
}
