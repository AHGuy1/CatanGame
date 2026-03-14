namespace CatanGame.Models
{
    public abstract class BoardModel
    {
        #region Enums
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
        #endregion

        #region Properties
        public VertexNode[] Vertices { get; set; } = [];
        public EdgeLink[] Edges { get; set; } = [];
        public HexTile[] Hexes { get; set; } = [];
        #endregion

        #region PublicMethods
        public abstract void InitBoard(IndexedButton[][] Pieces, string[] tileTypes, string[] tileNumbers);
        #endregion

        #region PrivateMethods
        protected abstract void InitHex(string[] tileTypes, string[] tileNumbers);
        protected abstract void InitEdges();
        protected abstract void InitVertices();
        protected abstract void SetEdgesVertices();
        protected abstract void SetVerticesEdegs();
        protected abstract void SetHexesVertices();
        #endregion
    }
}
