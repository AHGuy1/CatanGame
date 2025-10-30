namespace CatanGame.Models
{
    public class GameSize(int size)
    {
        public int Size { get; set; } = size;
        public string DisplayName { get; } = $"{size} + {size}";
    }
}
