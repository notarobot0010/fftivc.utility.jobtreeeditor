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
            PopulateSwapSelector(vm);
            _suppressSelectionSync = false;
        }
    }

    private void PopulateEditFields(JobViewModel vm)
    {
        XBox.Text = vm.X.ToString();
        YBox.Text = vm.Y.ToString();
    }

    private void PopulateSwapSelector(JobViewModel selected)
    {
        var others = _jobs.Where(j => j != selected).ToList();
        SwapSelector.ItemsSource = others;
        if (others.Count > 0)
            SwapSelector.SelectedIndex = 0;
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
            "Reset ALL jobs to their default positions?",
            "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        _uib.ResetAllToDefaults();
        RefreshAllViewModels();
        MarkChanged();

        StatusMessage?.Invoke("All jobs reset to default positions.");
    }

    private void ApplyManual_Click(object sender, RoutedEventArgs e)
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

    private void ResetSelected_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel vm) return;

        _uib.ResetToDefault(vm.Slot);
        vm.RefreshFrom(_uib.ReadPosition(vm.Slot));
        PopulateEditFields(vm);
        MarkChanged();

        StatusMessage?.Invoke($"{vm.Name} reset to default ({vm.Slot.DefaultX}, {vm.Slot.DefaultY}).");
    }

    private void Swap_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel sourceVm) return;
        if (SwapSelector.SelectedItem is not JobViewModel targetVm) return;

        if (sourceVm == targetVm) return;

        _uib.SwapPositions(sourceVm.Slot, targetVm.Slot);
        sourceVm.RefreshFrom(_uib.ReadPosition(sourceVm.Slot));
        targetVm.RefreshFrom(_uib.ReadPosition(targetVm.Slot));
        PopulateEditFields(sourceVm);
        MarkChanged();

        StatusMessage?.Invoke($"Swapped {sourceVm.Name} ({sourceVm.X}, {sourceVm.Y}) ↔ {targetVm.Name} ({targetVm.X}, {targetVm.Y}).");
    }
    #endregion

    private void RefreshAllViewModels()
    {
        if (_uib == null) return;

        var positions = _uib.ReadAllPositions();

        _jobs.Clear();
        foreach (var slot in UibConstants.Jobs)
            _jobs.Add(new JobViewModel(slot, positions[slot]));

        JobSelector.ItemsSource = _jobs;
        if (_jobs.Count > 0)
        {
            JobSelector.SelectedIndex = 0;
            PopulateSwapSelector(_jobs[0]);
        }
    }

    private void MarkChanged()
    {
        DataChanged?.Invoke();
        // Force the grid to re-evaluate row styling
        JobGrid.Items.Refresh();
    }
}
