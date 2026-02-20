namespace CatanGame.Models
{
    public abstract class BoardModel
    {
        public enum PieceType
        {
            None,
            Town,
            City
        }

        public enum TerrainType
        {
            None,
            Fields,
            Mountien,
            Forest,
            Hills,
            Pasture,
            Desert
        }

        public VertexNode[] Vertices { get; set; } = [];
        public EdgeLink[] Edges { get; set; } = [];
        public HexTile[] Hexes { get; set; } = [];

        protected abstract void InitHex(string[] tileTypes, string[] tileNumbers);
        protected abstract void InitEdges();
        protected abstract void InitVertices();
        protected abstract void SetEdgesVertices();
        protected abstract void SetVerticesEdegs();
        protected abstract void SetHexesVertices();

        public abstract void InitBoard(IndexedButton[][] Pieces, string[] tileTypes, string[] tileNumbers);
    }
}