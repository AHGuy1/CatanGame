using Google.Android.Material.Shape;

namespace CatanGame.Models
{
    public class IndexedButton : Button
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public IndexedButton(int rowIndex, int columnIndex,int heightRequest, int widthRequest,int rotation=0)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            HeightRequest = heightRequest;
            WidthRequest = widthRequest;
            Rotation = rotation;
            Background = Colors.Transparent;
            BorderColor = Colors.White;
            BorderWidth = 0;
            HorizontalOptions = LayoutOptions.Center;
            CornerRadius = 30;
        }
    }
}
