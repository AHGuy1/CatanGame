namespace CatanGame.Models
{
    public class EdgeLink(int row, int column)
    {
        #region Properties
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;
        public VertexNode VertexNodeOne { get; set; } = new();
        public VertexNode VertexNodeTwo { get; set; } = new();
        public int RoadOwnerPlayerIndex { get; set; } = -1;
        #endregion
    }
}
