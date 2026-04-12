using CatanGame.ModelsLogic;

namespace CatanGame.Models
{
    public class IndexedImageButton : ImageButton
    {
        #region Properties
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        #endregion

        #region Constructor
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
        #endregion
    }
}
