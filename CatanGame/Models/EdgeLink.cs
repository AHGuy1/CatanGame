namespace CatanGame.Models
{
    public abstract class EdgeLinkModel
    {
        public int Id { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int RoadOwnerPlayerIndex { get; set; }

        public EdgeLink()
        {
            RoadOwnerPlayerIndex = -1;
        }

        public EdgeLink(int id, int a, int b)
        {
            Id = id;
            A = a;
            B = b;
            RoadOwnerPlayerIndex = -1;
        }
    }
}