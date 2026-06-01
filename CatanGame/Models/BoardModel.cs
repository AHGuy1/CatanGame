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
        // Initializes the board graph from tile data.
        public abstract void InitBoard(IndexedButton[][] Pieces, string[] tileTypes, string[] tileNumbers);
        #endregion

        #region PrivateMethods
        // Maps a tile image name to its terrain type.
        protected static TerrainType GetTerrainFromTileType(string tileType) { if(Enum.TryParse(tileType, out TerrainType result)) return result; return TerrainType.None; }
        // Maps a number token image name to its dice value.
        protected static int GetNumberTokenFromTile(string tileNumber) { if (int.TryParse(tileNumber, out int result)) return result; return 0; }
        // Creates the hex tile nodes.
        protected abstract void InitHex(string[] tileTypes, string[] tileNumbers);
        // Creates the board edge links.
        protected abstract void InitEdges();
        // Creates the board vertex nodes.
        protected abstract void InitVertices();
        // Connects edges to their vertices.
        protected abstract void SetEdgesVertices();
        // Connects vertices to their edges.
        protected abstract void SetVerticesEdegs();
        // Connects hexes to their corner vertices.
        protected abstract void SetHexesVertices();
        #endregion
    }
}
