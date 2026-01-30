using static Android.InputMethodServices.Keyboard;

namespace CatanGame.Models
{
    public class HexTile
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public BoardModel.TerrainType Terrain { get; set; }
        public int NumberToken { get; set; }
        public int[] Corners { get; set; } = [];

        public HexTile()
        {
            Terrain = BoardModel.TerrainType.None;
            NumberToken = 0;
        }

        public HexTile(int row, int column, BoardModel.TerrainType terrainType, int numberToken)
        {
            Row = row;
            Column = column;
            Terrain = terrainType;
            NumberToken = numberToken;
        }
    }
}