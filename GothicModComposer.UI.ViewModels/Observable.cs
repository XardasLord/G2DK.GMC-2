using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GothicModComposer.UI.ViewModels
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, newValue))
                return false;

            storage = newValue;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
