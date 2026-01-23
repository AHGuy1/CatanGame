using CatanGame.Models;

namespace CatanGame.ModelsLogic
{
    public class Board : BoardModel
    {
        public Board()
        {
        }

        public override void InitEmpty(int vertexCount, int edgeCount, int hexCount)
        {
            Vertices = new VertexNode[vertexCount];
            for (int i = 0; i < vertexCount; i++)
                Vertices[i] = new VertexNode(i);

            Edges = new EdgeLink[edgeCount];
            for (int i = 0; i < edgeCount; i++)
                Edges[i] = new EdgeLink(i, -1, -1);

            Hexes = new HexTile[hexCount];
            for (int i = 0; i < hexCount; i++)
                Hexes[i] = new HexTile(i, BoardModel.TerrainType.None, 0, []);
        }

        public VertexNode GetVertex(int id)
        {
            return Vertices[id];
        }

        public EdgeLink GetEdge(int id)
        {
            return Edges[id];
        }

        public HexTile GetHex(int id)
        {
            return Hexes[id];
        }
    }
}