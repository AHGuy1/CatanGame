namespace CatanGame.Models
{
    public abstract class HexTile
    {
        public int Id { get; set; }
        public BoardModel.TerrainType Terrain { get; set; }
        public int NumberToken { get; set; }
        public int[] Corners { get; set; } = [];

        public HexTile()
        {
            Terrain = BoardModel.TerrainType.None;
            NumberToken = 0;
        }

        public HexTile(int id, BoardModel.TerrainType terrain, int numberToken, int[] corners)
        {
            Id = id;
            Terrain = terrain;
            NumberToken = numberToken;
            Corners = corners;
        }
    }
}