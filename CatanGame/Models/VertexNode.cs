using static Android.InputMethodServices.Keyboard;

namespace CatanGame.Models
{
    public class VertexNode
    {
        #region Properties
        public int Row { get; set; }
        public int Column { get; set; }
        public int PlayerIndex { get; set; } = -1;
        public BoardModel.PieceType PieceType { get; set; } = BoardModel.PieceType.None;
        public EdgeLink[] Edges { get; set; } = [];
        #endregion

        #region Constructor
        public VertexNode()
        {
        }

        public VertexNode(int row, int column)
        {
            Row = row;
            Column = column;
        }
        #endregion
    }
}
