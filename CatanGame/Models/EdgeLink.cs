namespace CatanGame.Models
{
    public class EdgeLink(int row, int column, VertexNode vertexModeOne, VertexNode vertexModeTwo)
    {
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;
        public VertexNode VertexModeOne { get; set; } = vertexModeOne;
        public VertexNode VertexModeTwo { get; set; } = vertexModeTwo;
        public int RoadOwnerPlayerIndex { get; set; } = -1;
    }
}