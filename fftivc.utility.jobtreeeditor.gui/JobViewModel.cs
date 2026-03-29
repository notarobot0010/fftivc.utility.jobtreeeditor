using fftivc.utility.jobtreeeditor.uib;
using System.ComponentModel;
using static fftivc.utility.jobtreeeditor.uib.UibConstants;

namespace fftivc.utility.jobtreeeditor.gui;

/// <summary>
/// View model for a single job row in the DataGrid.
/// Implements INotifyPropertyChanged so the grid updates when values change.
/// </summary>
public class JobViewModel(JobSlot slot, JobPosition pos, JobNameRef nameRef) : INotifyPropertyChanged
{
    public JobSlot Slot { get; } = slot;

    private int _x = pos.X;
    private int _y = pos.Y;
    private UibSlotName _slotBinding = AddressToSlotName.GetValueOrDefault(nameRef.Value);

    public string Name => Slot.Name;
    public int RecordIndex => Slot.RecordIndex;
    public int GeneralJobKey => Slot.GeneralJobKey;
    public string DefaultPosition => $"({Slot.DefaultX}, {Slot.DefaultY})";
    public UibSlotName DefaultSlotBinding =>
        AddressToSlotName.GetValueOrDefault(Slot.DefaultNameRef);

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

    public UibSlotName SlotBinding
    {
        get => _slotBinding;
        set
        {
            _slotBinding = value;
            OnPropertyChanged(nameof(SlotBinding));
            OnPropertyChanged(nameof(IsModified));
        }
    }

    public string CurrentPosition => $"({X}, {Y})";

    public bool IsModified => X != Slot.DefaultX || Y != Slot.DefaultY ||
        _slotBinding != DefaultSlotBinding;

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

    /// <summary>
    /// Refresh the slot binding from a name ref without triggering a file write.
    /// </summary>
    public void RefreshFrom(JobNameRef nameRef)
    {
        _slotBinding = AddressToSlotName.GetValueOrDefault(nameRef.Value);
        OnPropertyChanged(nameof(SlotBinding));
        OnPropertyChanged(nameof(IsModified));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
