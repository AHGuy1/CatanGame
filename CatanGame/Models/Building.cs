namespace CatanGame.Models
{
    public class Building
    {
        public int PlayerIndex { get; set; }
        public BoardModel.PieceType Type { get; set; }

        public Building()
        {
            Type = BoardModel.PieceType.None;
        }

        public Building(int playerIndex, BoardModel.PieceType type)
        {
            PlayerIndex = playerIndex;
            Type = type;
        }
    }
}