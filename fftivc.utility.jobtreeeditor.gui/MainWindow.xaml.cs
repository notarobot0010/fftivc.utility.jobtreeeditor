using fftivc.utility.jobtreeeditor.shared.Enums;
using fftivc.utility.jobtreeeditor.shared.Layout;
using fftivc.utility.jobtreeeditor.uib;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace fftivc.utility.jobtreeeditor.gui;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool _hasUnsavedChanges;
    private bool _hasUnexportedChanges;

    public MainWindow()
    {
        InitializeComponent();

        JobTreeEditor.DataChanged += () => { _hasUnsavedChanges = true; };
        GeneralJobEditor.DataChanged += () => { _hasUnexportedChanges = true; };

        JobTreeEditor.StatusMessage += msg => SetStatus(msg);

        JobTreeEditor.JobsSwapped += (keyA, keyB) =>
        {
            GeneralJobEditor.OnJobsSwapped((GeneralJobKey)keyA, (GeneralJobKey)keyB);
        };

        OutputPathBox.Text = Path.Combine(".", "output", "ffto_job_tree.uib");
        
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
            var uib = new JobTreeUib(path);
            JobTreeEditor.LoadUib(uib);
            InputPathBox.Text = path;
            _hasUnsavedChanges = false;

            SetStatus($"Loaded {Path.GetFileName(path)} — {JobTreeEditor.JobCount()} jobs.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load file:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void GenerateSql_Click(object sender, RoutedEventArgs e)
    {
        var choice = MessageBox.Show("Generate SQL for all values, or only changes from defaults?\n\n" + 
            "• Yes = All values\n" +
            "• No = Only changes\n" +
            "• Cancel = Abort",
            "SQL Export Mode",
        MessageBoxButton.YesNoCancel,
        MessageBoxImage.Question);

        if (choice == MessageBoxResult.Cancel) return;

        bool includeAllValues = (choice == MessageBoxResult.Yes);

        string sql = GeneralJobEditor.GenerateSql(includeAllValues);

        if (string.IsNullOrEmpty(sql))
        {
            MessageBox.Show("No GeneralJob changes to export.", "No Changes",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dlg = new SaveFileDialog
        {
            Title = "Save SQL Script",
            Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
            FileName = "generaljob_edits.sql",
        };

        if (dlg.ShowDialog() != true) return;

        try
        {
            File.WriteAllText(dlg.FileName, sql);
            SetStatus($"SQL exported to {dlg.FileName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save SQL:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var uib = JobTreeEditor.GetUibFile();
        if (uib == null) return;

        string outputPath = OutputPathBox.Text.Trim();
        if (string.IsNullOrEmpty(outputPath))
        {
            MessageBox.Show("Please specify an output path.", "No Output Path",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            uib.Save(outputPath);
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

    #region Layout

    private void LoadLayout_Click(object sender, RoutedEventArgs e)
    {
        var uib = JobTreeEditor.GetUibFile();
        if (uib == null)
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
            var records = GeneralJobEditor.GetRecords();
            
            var result = MessageBox.Show(
                $"Apply layout \"{config.Name}\"?\n{config.Jobs.Count} positions defined.",
                "Confirm Load", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            var (applied, skipped) = config.Apply(uib, records);

            JobTreeEditor.ApplyLayout();
            GeneralJobEditor.RefreshViewModels();

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
        var uib = JobTreeEditor.GetUibFile();
        if (uib == null)
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
            var records = GeneralJobEditor.GetRecords();
            var config = LayoutConfig.Export(uib, records, "Exported Layout");
            config.SaveToFile(dlg.FileName);
            _hasUnexportedChanges = false;
            SetStatus($"Layout exported to {dlg.FileName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to export:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ResetAll_Click(object sender, RoutedEventArgs e) 
    {
        JobTreeEditor.ResetAll();
    }
    #endregion

    #region Helpers
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

        if (_hasUnexportedChanges)
        {
            var result = MessageBox.Show(
                "You have unexported changes. Export layout before closing?",
                "Unexported Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ExportLayout_Click(this, new RoutedEventArgs());
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