using fftivc.utility.jobtreeeditor.uib;
using System.ComponentModel;

namespace fftivc.utility.jobtreeeditor.gui;

/// <summary>
/// View model for a single job row in the DataGrid.
/// Implements INotifyPropertyChanged so the grid updates when values change.
/// </summary>
public class JobViewModel : INotifyPropertyChanged
{
    public JobSlot Slot { get; }

    private int _x;
    private int _y;

    public string Name => Slot.Name;
    public int RecordIndex => Slot.RecordIndex;
    public int GeneralJobKey => Slot.GeneralJobKey;
    public string DefaultPosition => $"({Slot.DefaultX}, {Slot.DefaultY})";

    public int X
    {
        get => _x;
        set { 
            _x = value; 
            OnPropertyChanged(nameof(X)); 
            OnPropertyChanged(nameof(IsModified)); 
            OnPropertyChanged(nameof(CurrentPosition)); 
        }
    }

    public int Y
    {
        get => _y;
        set { 
            _y = value; 
            OnPropertyChanged(nameof(Y)); 
            OnPropertyChanged(nameof(IsModified)); 
            OnPropertyChanged(nameof(CurrentPosition)); 
        }
    }

    public string CurrentPosition => $"({X}, {Y})";

    public bool IsModified => X != Slot.DefaultX || Y != Slot.DefaultY;

    public JobViewModel(JobSlot slot, JobPosition pos)
    {
        Slot = slot;
        _x = pos.X;
        _y = pos.Y;
    }

    /// <summary>
    /// Refresh X and Y from the given position without triggering a file write.
    /// </summary>
    public void RefreshFrom(JobPosition pos)
    {
        _x = pos.X;
        _y = pos.Y;
        OnPropertyChanged(nameof(X));
        OnPropertyChanged(nameof(Y));
        OnPropertyChanged(nameof(IsModified));
        OnPropertyChanged(nameof(CurrentPosition));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
