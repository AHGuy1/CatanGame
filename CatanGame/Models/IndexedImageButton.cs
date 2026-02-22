using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public class IndexedImageButton : ImageButton
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public IndexedImageButton(int rowIndex, int columnIndex, double size)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            BorderColor = Colors.White;
            HeightRequest = size;
            WidthRequest = size;
            CornerRadius = 90;
            BorderWidth = 0;
        }
    }
}
