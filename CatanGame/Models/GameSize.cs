namespace CatanGame.Models
{
    public class GameSize
    {
        #region Properties
        public int Size { get; set; }
        public string DisplayName => $"{Size}" + Strings.EmptySpace + Strings.PlayersLabel;
        #endregion

        #region Constructor
        public GameSize(int size)
        {
            Size = size;
        }

        public GameSize()
        {
            Size = 4;
        }
        #endregion
    }
}
