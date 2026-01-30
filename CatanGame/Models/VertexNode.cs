using static Android.InputMethodServices.Keyboard;

namespace CatanGame.Models
{
    public class VertexNode
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Building? Building { get; set; }
        public EdgeLink[] Edges { get; set; } = [];

        public VertexNode()
        {
        }

        public VertexNode(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}