using fftivc.utility.jobtreeeditor.uib;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace fftivc.utility.jobtreeeditor.gui;
/// <summary>
/// Interaction logic for JobTreePositions.xaml
/// </summary>
public partial class JobTreePositionsControl : UserControl
{
    private JobTreeUib? _uib;
    private ObservableCollection<JobViewModel> _jobs = [];
    private bool _suppressSelectionSync;

    public event Action? DataChanged;
    public event Action<string>? StatusMessage;
    public event Action<int, int>? JobsSwapped;

    public JobTreeUib? GetUibFile() => _uib;
    public int JobCount() => _jobs.Count;

    public JobTreePositionsControl()
    {
        InitializeComponent();
        JobGrid.ItemsSource = _jobs;
    }

    public void LoadUib(JobTreeUib uib)
    {
        _uib = uib;
        RefreshAllViewModels();
        EditPanel.IsEnabled = true;
    }

    #region ModeToggle
    private void Mode_Changed(object sender, RoutedEventArgs e)
    {
        if (BindingsPanel == null || PositionsPanel == null) return;

        if (ModeBindings.IsChecked == true)
        {
            BindingsPanel.Visibility = Visibility.Visible;
            PositionsPanel.Visibility = Visibility.Collapsed;
        }
        else
        {
            BindingsPanel.Visibility = Visibility.Collapsed;
            PositionsPanel.Visibility = Visibility.Visible;
        }
    }
    #endregion

    #region GridSelection
    private void JobGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressSelectionSync) return;

        if (JobGrid.SelectedItem is JobViewModel vm)
        {
            _suppressSelectionSync = true;
            JobSelector.SelectedItem = vm;
            PopulateEditFields(vm);
            _suppressSelectionSync = false;
        }
    }

    private void JobSelector_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressSelectionSync) return;

        if (JobSelector.SelectedItem is JobViewModel vm)
        {
            _suppressSelectionSync = true;
            JobGrid.SelectedItem = vm;
            JobGrid.ScrollIntoView(vm);
            PopulateEditFields(vm);
            PopulateSwapSelectors(vm);
            _suppressSelectionSync = false;
        }
    }

    private void PopulateEditFields(JobViewModel vm)
    {
        XBox.Text = vm.X.ToString();
        YBox.Text = vm.Y.ToString();
        CurrentBindingLabel.Text = vm.SlotBinding.ToString();
    }

    private void PopulateSwapSelectors(JobViewModel selected)
    {
        var others = _jobs.Where(j => j != selected).ToList();
        PositionSwapSelector.ItemsSource = others;
        BindingSwapSelector.ItemsSource = others;
        if (others.Count > 0)
        {
            PositionSwapSelector.SelectedIndex = 0;
            BindingSwapSelector.SelectedIndex = 0;
        }
    }
    #endregion

    #region EditActions
    public void ApplyLayout()
    {
        RefreshAllViewModels();
        MarkChanged();
    }

    public void ResetAll()
    {
        if (_uib == null) return;

        var result = MessageBox.Show(
            "Reset ALL jobs to their default positions and slot bindings?",
            "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        _uib.ResetAllPositionsToDefaults();
        _uib.ResetAllNameRefsToDefaults();

        RefreshAllViewModels();
        MarkChanged();

        StatusMessage?.Invoke("All jobs reset to defaults.");
    }

    #region Slot Bindings

    private void SwapBindings_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel sourceVm) return;
        if (BindingSwapSelector.SelectedItem is not JobViewModel targetVm) return;
        if (sourceVm == targetVm) return;

        var refA = _uib.ReadNameRef(sourceVm.Slot);
        var refB = _uib.ReadNameRef(targetVm.Slot);
        _uib.WriteNameRef(sourceVm.Slot, refB);
        _uib.WriteNameRef(targetVm.Slot, refA);
        sourceVm.RefreshFrom(_uib.ReadNameRef(sourceVm.Slot));
        targetVm.RefreshFrom(_uib.ReadNameRef(targetVm.Slot));
        
        PopulateEditFields(sourceVm);
        MarkChanged();

        StatusMessage?.Invoke($"Swapped bindings: {sourceVm.Name} → {sourceVm.SlotBinding}, {targetVm.Name} → {targetVm.SlotBinding}.");
    }

    private void ResetBinding_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel vm) return;

        _uib.ResetNameRefToDefault(vm.Slot);
        vm.RefreshFrom(_uib.ReadNameRef(vm.Slot));
        PopulateEditFields(vm);
        MarkChanged();

        StatusMessage?.Invoke($"{vm.Name} binding reset to {vm.DefaultSlotBinding}.");
    }
    #endregion

    #region Positions

    private void ApplyPosition_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel vm) return;

        if (!int.TryParse(XBox.Text.Trim(), out int x))
        {
            MessageBox.Show("Invalid X value.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!int.TryParse(YBox.Text.Trim(), out int y))
        {
            MessageBox.Show("Invalid Y value.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (x < 0 || x > UibConstants.ScreenWidth || y < 0 || y > UibConstants.ScreenHeight)
        {
            var result = MessageBox.Show(
                $"({x}, {y}) is outside the standard 1920×1080 area.\nApply anyway?",
                "Out of Range", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
        }

        _uib.WritePosition(vm.Slot, new JobPosition(x, y));
        vm.RefreshFrom(_uib.ReadPosition(vm.Slot));
        MarkChanged();

        StatusMessage?.Invoke($"{vm.Name} moved to ({x}, {y}).");
    }

    private void ResetPosition_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel vm) return;

        _uib.ResetPositionToDefault(vm.Slot);
        vm.RefreshFrom(_uib.ReadPosition(vm.Slot));
        PopulateEditFields(vm);
        MarkChanged();

        StatusMessage?.Invoke($"{vm.Name} position reset to ({vm.Slot.DefaultX}, {vm.Slot.DefaultY}).");
    }

    private void SwapPositions_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel sourceVm) return;
        if (PositionSwapSelector.SelectedItem is not JobViewModel targetVm) return;
        if (sourceVm == targetVm) return;

        _uib.SwapPositions(sourceVm.Slot, targetVm.Slot);
        sourceVm.RefreshFrom(_uib.ReadPosition(sourceVm.Slot));
        targetVm.RefreshFrom(_uib.ReadPosition(targetVm.Slot));
        PopulateEditFields(sourceVm);
        MarkChanged();

        JobsSwapped?.Invoke(sourceVm.Slot.GeneralJobKey, targetVm.Slot.GeneralJobKey);
        StatusMessage?.Invoke($"Swapped positions: {sourceVm.Name} ({sourceVm.X}, {sourceVm.Y}) ↔ {targetVm.Name} ({targetVm.X}, {targetVm.Y}).");
    }
    #endregion
    #endregion

    private void RefreshAllViewModels()
    {
        if (_uib == null) return;

        var positions = _uib.ReadAllPositions();
        var nameRefs = _uib.ReadAllNameRefs();

        _jobs.Clear();
        foreach (var slot in UibConstants.Jobs)
            _jobs.Add(new JobViewModel(slot, positions[slot], nameRefs[slot]));

        JobSelector.ItemsSource = _jobs;
        if (_jobs.Count > 0)
        {
            JobSelector.SelectedIndex = 0;
            PopulateSwapSelectors(_jobs[0]);
        }
    }

    private void MarkChanged()
    {
        DataChanged?.Invoke();
        JobGrid.Items.Refresh();
    }
}
