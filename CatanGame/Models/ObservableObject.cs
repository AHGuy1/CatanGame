using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CatanGame.Models
{
    public partial class ObservableObject : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region PrivateMethods
        // Raises a property changed notification.
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

