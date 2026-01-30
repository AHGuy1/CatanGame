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

        protected abstract void InitHex(string[] tileTypes, string[] tileNumbers);
        protected abstract void InitEdges();
        protected abstract void InitVertices();
        protected abstract void SetEdgesVertices();
        protected abstract void SetVerticesEdegs();


        public abstract void InitBoard(IndexedButton[][] pices, string[] tileTypes, string[] tileNumbers);
    }
}