using fftivc.utility.jobtreeeditor.uib;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace fftivc.utility.jobtreeeditor.gui;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private JobTreeUibEditor? _uib;
    private ObservableCollection<JobViewModel> _jobs = [];
    private bool _hasUnsavedChanges;
    private bool _suppressSelectionSync;

    public MainWindow()
    {
        InitializeComponent();

        OutputPathBox.Text = Path.Combine(".", "output", "ffto_job_tree.uib");
        JobGrid.ItemsSource = _jobs;

        SetStatus("Open a .uib file to get started.");
    }
    
    #region FileIO
    private void BrowseInput_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            Title = "Open Job Tree UIB",
            Filter = "UIB files (*.uib)|*.uib|All files (*.*)|*.*",
            FileName = "ffto_job_tree.uib",
        };

        if (dlg.ShowDialog() == true)
            LoadFile(dlg.FileName);
    }

    private void BrowseOutput_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new SaveFileDialog
        {
            Title = "Choose Output Path",
            Filter = "UIB files (*.uib)|*.uib|All files (*.*)|*.*",
            FileName = "ffto_job_tree.uib",
        };

        if (dlg.ShowDialog() == true)
            OutputPathBox.Text = dlg.FileName;
    }

    private void LoadFile(string path)
    {
        try
        {
            _uib = new JobTreeUibEditor(path);
            InputPathBox.Text = path;

            RefreshAllViewModels();
            EditPanel.IsEnabled = true;
            _hasUnsavedChanges = false;

            SetStatus($"Loaded {Path.GetFileName(path)} — {_jobs.Count} jobs.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load file:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;

        string outputPath = OutputPathBox.Text.Trim();
        if (string.IsNullOrEmpty(outputPath))
        {
            MessageBox.Show("Please specify an output path.", "No Output Path",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            _uib.Save(outputPath);
            _hasUnsavedChanges = false;
            SetStatus($"Saved to {outputPath}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
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

        SetStatus($"{vm.Name} moved to ({x}, {y}).");
    }

    private void ResetSelected_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;
        if (JobSelector.SelectedItem is not JobViewModel vm) return;

        _uib.ResetToDefault(vm.Slot);
        vm.RefreshFrom(_uib.ReadPosition(vm.Slot));
        PopulateEditFields(vm);
        MarkChanged();

        SetStatus($"{vm.Name} reset to default ({vm.Slot.DefaultX}, {vm.Slot.DefaultY}).");
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

        SetStatus($"Swapped {sourceVm.Name} ({sourceVm.X}, {sourceVm.Y}) ↔ {targetVm.Name} ({targetVm.X}, {targetVm.Y}).");
    }

    private void ResetAll_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null) return;

        var result = MessageBox.Show(
            "Reset ALL jobs to their default positions?",
            "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        _uib.ResetAllToDefaults();
        RefreshAllViewModels();
        MarkChanged();

        SetStatus("All jobs reset to default positions.");
    }
#endregion

    #region Layout

    private void LoadLayout_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null)
        {
            MessageBox.Show("Open a UIB file first.", "No File", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dlg = new OpenFileDialog
        {
            Title = "Load Layout JSON",
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
        };

        if (dlg.ShowDialog() != true) return;

        try
        {
            var config = LayoutConfig.LoadFromFile(dlg.FileName);

            var result = MessageBox.Show(
                $"Apply layout \"{config.Name}\"?\n{config.Positions.Count} positions defined.",
                "Confirm Load", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            var (applied, skipped) = config.ApplyTo(_uib);
            RefreshAllViewModels();
            MarkChanged();

            string msg = $"Applied {applied.Count} positions.";
            if (skipped.Count > 0)
                msg += $" Skipped: {string.Join(", ", skipped)}";
            SetStatus(msg);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load layout:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExportLayout_Click(object sender, RoutedEventArgs e)
    {
        if (_uib == null)
        {
            MessageBox.Show("Open a UIB file first.", "No File", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dlg = new SaveFileDialog
        {
            Title = "Export Layout JSON",
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            FileName = "layout.json",
        };

        if (dlg.ShowDialog() != true) return;

        try
        {
            var config = LayoutConfig.FromUibFile(_uib, "Exported Layout");
            config.SaveToFile(dlg.FileName);
            SetStatus($"Layout exported to {dlg.FileName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to export:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion

    #region Helpers
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
        _hasUnsavedChanges = true;
        // Force the grid to re-evaluate row styling
        JobGrid.Items.Refresh();
    }

    private void SetStatus(string text)
    {
        StatusText.Text = text;
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        if (_hasUnsavedChanges)
        {
            var result = MessageBox.Show(
                "You have unsaved changes. Save before closing?",
                "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Save_Click(this, new RoutedEventArgs());
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        base.OnClosing(e);
    }
    #endregion
}