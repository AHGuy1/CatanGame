using CatanGame.ModelsLogic;
using System;

namespace CatanGame.Models
{
    public abstract class GameGridModel : Grid
    {
        protected IndexedButton[][] BoardPiceButtons = new IndexedButton[24][];
        protected IndexedImage[][] BoardPiceImages = new IndexedImage[24][];
        public EventHandler<IndexedButton>? ButtonClicked;

        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        
        public abstract void OnChange();
        public abstract void Init(Grid gameBoard, Grid grdPices);
        public abstract void ShowBuildOptions(string peiceType, Game game);
    }
}
