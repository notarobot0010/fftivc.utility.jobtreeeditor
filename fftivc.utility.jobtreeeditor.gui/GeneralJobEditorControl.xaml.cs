using fftivc.utility.jobtreeeditor.shared;
using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.GeneralJob;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace fftivc.utility.jobtreeeditor.gui;
/// <summary>
/// Interaction logic for GeneralJobEditorControl.xaml
/// </summary>
public partial class GeneralJobEditorControl : UserControl
{
    private List<GeneralJobRecord> _records = [];
    private List<GeneralJobRecord> _originals = [];
    private ObservableCollection<GeneralJobViewModel> _viewModels = [];
    private ObservableCollection<PrereqViewModel> _prereqViewModels = [];
    private List<string> _jobRefList = [];
    private bool _suppressSelection;

    /// <summary>Raised when any data changes, so MainWindow can track unsaved state.</summary>
    public event Action? DataChanged;

    public List<GeneralJobRecord> GetRecords() => _records;

    public GeneralJobEditorControl()
    {
        InitializeComponent();
        BuildJobRefList();
        Initialize();
    }

    /// <summary>Generate SQL for all modifications.</summary>
    public string GenerateSql(bool includeAllValues)
    {
        return SqlGenerator.Generate(_records, GeneralJobDefaults.CreateDefaults(), includeAllValues);
    }

    private void BuildJobRefList()
    {
        _jobRefList = [];
        for (int i = 0; i <= 20; i++)
            _jobRefList.Add(GeneralJobDefaults.FormatJobRef(i));

        RightNeighborBox.ItemsSource = _jobRefList;
        DownNeighborBox.ItemsSource = _jobRefList;
        LeftNeighborBox.ItemsSource = _jobRefList;
        UpNeighborBox.ItemsSource = _jobRefList;
    }

    public void Initialize()
    {
        _originals = GeneralJobDefaults.CreateDefaults();
        _records = GeneralJobDefaults.CreateDefaults();
        RefreshViewModels();
    }

    public void RefreshViewModels()
    {
        _viewModels.Clear();
        foreach (var rec in _records)
        {
            var orig = _originals.First(o => o.Key == rec.Key);
            _viewModels.Add(new GeneralJobViewModel(rec, orig));
        }

        GjGrid.ItemsSource = _viewModels;
        GjJobSelector.ItemsSource = _viewModels;

        if (_viewModels.Count > 0)
        {
            GjJobSelector.SelectedIndex = 0;
            PopulateEditFields(_viewModels[0]);
        }
    }

    #region Selection Sync
    private void GjJobSelector_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressSelection) return;
        if (GjJobSelector.SelectedItem is GeneralJobViewModel vm)
        {
            _suppressSelection = true;
            GjGrid.SelectedItem = vm;
            GjGrid.ScrollIntoView(vm);
            PopulateEditFields(vm);
            _suppressSelection = false;
        }
    }

    private void GjGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressSelection) return;
        if (GjGrid.SelectedItem is GeneralJobViewModel vm)
        {
            _suppressSelection = true;
            GjJobSelector.SelectedItem = vm;
            PopulateEditFields(vm);
            _suppressSelection = false;
        }
    }

    private void PopulateEditFields(GeneralJobViewModel vm)
    {
        CommentBox.Text = vm.Record.Comment;
        RightNeighborBox.SelectedIndex = (int)vm.Record.RightNeighbor;
        DownNeighborBox.SelectedIndex = (int)vm.Record.DownNeighbor;
        LeftNeighborBox.SelectedIndex = (int)vm.Record.LeftNeighbor;
        UpNeighborBox.SelectedIndex = (int)vm.Record.UpNeighbor;

        _prereqViewModels.Clear();
        foreach (var p in vm.Record.Prerequisites)
            _prereqViewModels.Add(new PrereqViewModel(p));
        PrereqGrid.ItemsSource = _prereqViewModels;
    }
    #endregion

    #region Edit Actions
    public void OnJobsSwapped(Job jobA, Job jobB)
    {
        NeighborCalculator.SwapNeighborReferences(_records, jobA, jobB);

        foreach (var vm in _viewModels)
            vm.Refresh();
        GjGrid.Items.Refresh();

        if (GjJobSelector.SelectedItem is GeneralJobViewModel selected)
            PopulateEditFields(selected);

        DataChanged?.Invoke();
    }

    private void ApplyChanges_Click(object sender, RoutedEventArgs e)
    {
        if (GjJobSelector.SelectedItem is not GeneralJobViewModel vm) return;

        vm.Record.Comment = CommentBox.Text?.Trim() ?? "";
        vm.Record.RightNeighbor = (Job)RightNeighborBox.SelectedIndex;
        vm.Record.DownNeighbor = (Job)DownNeighborBox.SelectedIndex;
        vm.Record.LeftNeighbor = (Job)LeftNeighborBox.SelectedIndex;
        vm.Record.UpNeighbor = (Job)UpNeighborBox.SelectedIndex;

        vm.Record.Prerequisites.Clear();
        foreach (var pvm in _prereqViewModels)
            vm.Record.Prerequisites.Add(pvm.ToPrerequisite());

        vm.Refresh();
        GjGrid.Items.Refresh();
        DataChanged?.Invoke();
    }

    private void ResetSelected_Click(object sender, RoutedEventArgs e)
    {
        if (GjJobSelector.SelectedItem is not GeneralJobViewModel vm) return;

        var defaults = GeneralJobDefaults.CreateDefaults();
        var def = defaults.First(d => d.Key == vm.Record.Key);

        vm.Record.Comment = def.Comment;
        vm.Record.RightNeighbor = def.RightNeighbor;
        vm.Record.DownNeighbor = def.DownNeighbor;
        vm.Record.LeftNeighbor = def.LeftNeighbor;
        vm.Record.UpNeighbor = def.UpNeighbor;
        vm.Record.Prerequisites = [.. def.Prerequisites.Select(p => p.Clone())];
        vm.Record.RequiredJobExp = [.. def.RequiredJobExp];

        vm.Refresh();
        PopulateEditFields(vm);
        GjGrid.Items.Refresh();
        DataChanged?.Invoke();
    }

    private void AddPrereq_Click(object sender, RoutedEventArgs e)
    {
        _prereqViewModels.Add(new PrereqViewModel(
            new JobPrerequisite(Job.Squire, 2, LevelRequirementPosition.Bottom)));
    }

    private void RemovePrereq_Click(object sender, RoutedEventArgs e)
    {
        if (PrereqGrid.SelectedItem is PrereqViewModel pvm)
            _prereqViewModels.Remove(pvm);
    }

    private void ResetNeighbors_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Reset all cursor neighbors to their default values?\nThis will not affect prerequisites or other fields.",
            "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        var defaults = GeneralJobDefaults.CreateDefaults();
        foreach (var rec in _records)
        {
            var def = defaults.First(d => d.Key == rec.Key);
            rec.RightNeighbor = def.RightNeighbor;
            rec.DownNeighbor = def.DownNeighbor;
            rec.LeftNeighbor = def.LeftNeighbor;
            rec.UpNeighbor = def.UpNeighbor;
        }

        foreach (var vm in _viewModels)
            vm.Refresh();
        GjGrid.Items.Refresh();

        if (GjJobSelector.SelectedItem is GeneralJobViewModel selected)
            PopulateEditFields(selected);

        DataChanged?.Invoke();
    }

    private void ResetAll_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Reset ALL GeneralJob records to defaults?",
            "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        Initialize();
        DataChanged?.Invoke();
    }
    #endregion
}
