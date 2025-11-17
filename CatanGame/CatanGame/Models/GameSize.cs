namespace CatanGame.Models
{
    public class GameSize
    {
        public int Size { get; set; }
        public string DisplayName => $"{Size}" + Strings.EmptySpace + Strings.PlayersLabel;
        public GameSize(int size)
        {
            Size = size;
        }
        public GameSize()
        {
            Size = 4;
        }
    }
}
