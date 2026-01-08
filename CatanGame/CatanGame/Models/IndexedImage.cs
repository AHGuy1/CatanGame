namespace CatanGame.Models
{
    public class IndexedImage : Image
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public IndexedImage(int rowIndex, int columnIndex, int heightRequest, int widthRequest, int rotation = 0)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            HeightRequest = heightRequest;
            WidthRequest = widthRequest;
            Rotation = rotation;
        }
    }
}

