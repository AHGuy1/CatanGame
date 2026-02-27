using Google.Android.Material.Shape;

namespace CatanGame.Models
{
    public class IndexedButton : Button
    {
        //Row >= 1
        public int RowIndex { get; set; }
        //Column >= 1
        public int ColumnIndex { get; set; }
        public IndexedButton(int rowIndex, int columnIndex,double heightRequest, double widthRequest,int rotation=0)
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
            CornerRadius = 120;
        }
    }
}
