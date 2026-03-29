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

    /// <summary>All 16 neighbor combo boxes grouped for easy iteration.</summary>
    private ComboBox[] AllNeighborBoxes => [
        RightNeighborBox, RightFallbackBox,
        DownRightNeighborBox, DownRightFallbackBox,
        DownNeighborBox, DownFallbackBox,
        DownLeftNeighborBox, DownLeftFallbackBox,
        LeftNeighborBox, LeftFallbackBox,
        UpLeftNeighborBox, UpLeftFallbackBox,
        UpNeighborBox, UpFallbackBox,
        UpRightNeighborBox, UpRightFallbackBox,
    ];

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

        foreach (var box in AllNeighborBoxes)
            box.ItemsSource = _jobRefList;
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
        CompassCenterLabel.Text = vm.Name;

        // Primary neighbors
        RightNeighborBox.SelectedIndex = (int)vm.Record.RightNeighbor;
        DownRightNeighborBox.SelectedIndex = (int)vm.Record.DownRightNeighbor;
        DownNeighborBox.SelectedIndex = (int)vm.Record.DownNeighbor;
        DownLeftNeighborBox.SelectedIndex = (int)vm.Record.DownLeftNeighbor;
        LeftNeighborBox.SelectedIndex = (int)vm.Record.LeftNeighbor;
        UpLeftNeighborBox.SelectedIndex = (int)vm.Record.UpLeftNeighbor;
        UpNeighborBox.SelectedIndex = (int)vm.Record.UpNeighbor;
        UpRightNeighborBox.SelectedIndex = (int)vm.Record.UpRightNeighbor;

        // Fallback neighbors
        RightFallbackBox.SelectedIndex = (int)vm.Record.RightFallback;
        DownRightFallbackBox.SelectedIndex = (int)vm.Record.DownRightFallback;
        DownFallbackBox.SelectedIndex = (int)vm.Record.DownFallback;
        DownLeftFallbackBox.SelectedIndex = (int)vm.Record.DownLeftFallback;
        LeftFallbackBox.SelectedIndex = (int)vm.Record.LeftFallback;
        UpLeftFallbackBox.SelectedIndex = (int)vm.Record.UpLeftFallback;
        UpFallbackBox.SelectedIndex = (int)vm.Record.UpFallback;
        UpRightFallbackBox.SelectedIndex = (int)vm.Record.UpRightFallback;

        // Prerequisites
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

        // Primary neighbors
        vm.Record.RightNeighbor = (Job)RightNeighborBox.SelectedIndex;
        vm.Record.DownRightNeighbor = (Job)DownRightNeighborBox.SelectedIndex;
        vm.Record.DownNeighbor = (Job)DownNeighborBox.SelectedIndex;
        vm.Record.DownLeftNeighbor = (Job)DownLeftNeighborBox.SelectedIndex;
        vm.Record.LeftNeighbor = (Job)LeftNeighborBox.SelectedIndex;
        vm.Record.UpLeftNeighbor = (Job)UpLeftNeighborBox.SelectedIndex;
        vm.Record.UpNeighbor = (Job)UpNeighborBox.SelectedIndex;
        vm.Record.UpRightNeighbor = (Job)UpRightNeighborBox.SelectedIndex;

        // Fallback neighbors
        vm.Record.RightFallback = (Job)RightFallbackBox.SelectedIndex;
        vm.Record.DownRightFallback = (Job)DownRightFallbackBox.SelectedIndex;
        vm.Record.DownFallback = (Job)DownFallbackBox.SelectedIndex;
        vm.Record.DownLeftFallback = (Job)DownLeftFallbackBox.SelectedIndex;
        vm.Record.LeftFallback = (Job)LeftFallbackBox.SelectedIndex;
        vm.Record.UpLeftFallback = (Job)UpLeftFallbackBox.SelectedIndex;
        vm.Record.UpFallback = (Job)UpFallbackBox.SelectedIndex;
        vm.Record.UpRightFallback = (Job)UpRightFallbackBox.SelectedIndex;

        // Prerequisites
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

        // Primary neighbors
        vm.Record.RightNeighbor = def.RightNeighbor;
        vm.Record.DownRightNeighbor = def.DownRightNeighbor;
        vm.Record.DownNeighbor = def.DownNeighbor;
        vm.Record.DownLeftNeighbor = def.DownLeftNeighbor;
        vm.Record.LeftNeighbor = def.LeftNeighbor;
        vm.Record.UpLeftNeighbor = def.UpLeftNeighbor;
        vm.Record.UpNeighbor = def.UpNeighbor;
        vm.Record.UpRightNeighbor = def.UpRightNeighbor;

        // Fallback neighbors
        vm.Record.RightFallback = def.RightFallback;
        vm.Record.DownRightFallback = def.DownRightFallback;
        vm.Record.DownFallback = def.DownFallback;
        vm.Record.DownLeftFallback = def.DownLeftFallback;
        vm.Record.LeftFallback = def.LeftFallback;
        vm.Record.UpLeftFallback = def.UpLeftFallback;
        vm.Record.UpFallback = def.UpFallback;
        vm.Record.UpRightFallback = def.UpRightFallback;

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
            "Reset all cursor neighbors (all 8 directions + fallbacks) to their default values?\n" +
            "This will not affect prerequisites or other fields.",
            "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        var defaults = GeneralJobDefaults.CreateDefaults();
        foreach (var rec in _records)
        {
            var def = defaults.First(d => d.Key == rec.Key);

            rec.RightNeighbor = def.RightNeighbor;
            rec.RightFallback = def.RightFallback;
            rec.DownRightNeighbor = def.DownRightNeighbor;
            rec.DownRightFallback = def.DownRightFallback;
            rec.DownNeighbor = def.DownNeighbor;
            rec.DownFallback = def.DownFallback;
            rec.DownLeftNeighbor = def.DownLeftNeighbor;
            rec.DownLeftFallback = def.DownLeftFallback;
            rec.LeftNeighbor = def.LeftNeighbor;
            rec.LeftFallback = def.LeftFallback;
            rec.UpLeftNeighbor = def.UpLeftNeighbor;
            rec.UpLeftFallback = def.UpLeftFallback;
            rec.UpNeighbor = def.UpNeighbor;
            rec.UpFallback = def.UpFallback;
            rec.UpRightNeighbor = def.UpRightNeighbor;
            rec.UpRightFallback = def.UpRightFallback;
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
