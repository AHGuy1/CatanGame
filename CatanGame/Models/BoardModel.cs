namespace CatanGame.Models
{
    public abstract class BoardModel
    {
        public enum PieceType
        {
            None = 0,
            Road = 1,
            Town = 2,
            City = 3
        }

        public enum TerrainType
        {
            None = 0,
            Fields = 1,
            Mountien = 2,
            Forest = 3,
            Hills = 4,
            Pasture = 5,
            Desert = 6
        }

        public VertexNode[] Vertices { get; set; } = [];
        public EdgeLink[] Edges { get; set; } = [];
        public HexTile[] Hexes { get; set; } = [];

        public abstract void InitEmpty(int vertexCount, int edgeCount, int hexCount);
    }
}