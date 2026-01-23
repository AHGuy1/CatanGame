namespace CatanGame.Models
{
    public abstract class VertexNodeModel
    {
        public int Id { get; set; }
        public Building? Building { get; set; }
        public int[] Edges { get; set; } = [];

        public VertexNode()
        {
        }

        public VertexNode(int id)
        {
            Id = id;
        }
    }
}