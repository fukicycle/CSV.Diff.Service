using System.Collections.Immutable;
using System.Windows.Input;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Wpf.Commands;

namespace CSV.Diff.Service.Wpf.ViewModels;

public sealed class MainWindowViewModel : ViewModel
{
    private FilePath _previousFile = FilePath.Empty;
    private FilePath _afterFile = FilePath.Empty;
    private PreviewData _previousData = PreviewData.Empty;
    private PreviewData _afterData = PreviewData.Empty;
    private ImmutableList<string> _columnList = ImmutableList<string>.Empty;
    private ImmutableList<string> _targetColumnList = ImmutableList<string>.Empty;

    public MainWindowViewModel()
    {
        PreviousFileBrowseCommand = new PreviousFileBrowseCommand(this);
        AfterFileBrowseCommand = new AfterFileBrowseCommand(this);
    }

    public FilePath PreviousFile
    {
        get => _previousFile;
        set
        {
            SetProperty(ref _previousFile, value);
        }
    }
    public FilePath AfterFile
    {
        get => _afterFile;
        set
        {
            SetProperty(ref _afterFile, value);
        }
    }

    public PreviewData PreviousData
    {
        get => _previousData;
        set
        {
            SetProperty(ref _previousData, value);
        }
    }
    public PreviewData AfterData
    {
        get => _afterData;
        set
        {
            SetProperty(ref _afterData, value);
        }
    }

    public ImmutableList<string> ColumnList
    {
        get => _columnList;
        set
        {
            SetProperty(ref _columnList, value);
        }
    }
    public ImmutableList<string> TargetColumnList
    {
        get => _targetColumnList;
        set
        {
            SetProperty(ref _targetColumnList, value);
        }
    }

    public ICommand PreviousFileBrowseCommand { get; }
    public ICommand AfterFileBrowseCommand { get; }
}
