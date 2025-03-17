using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSV.Diff.Service.Wpf.ViewModels;

public abstract class ViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Property changed
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value))
        {
            return false;
        }
        field = value;
        var h = this.PropertyChanged;
        if (h != null)
        {
            h(this, new PropertyChangedEventArgs(propertyName));
        }
        return true;
    }

    public void OnPropertyChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
}
