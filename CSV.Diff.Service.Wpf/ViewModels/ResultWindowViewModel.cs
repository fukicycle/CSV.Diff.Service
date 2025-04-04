using System.Windows.Input;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Wpf.Commands;

namespace CSV.Diff.Service.Wpf.ViewModels;

public sealed class ResultWindowViewModel : ViewModel
{
    private IEnumerable<DiffResultContent> _added = Enumerable.Empty<DiffResultContent>();
    private IEnumerable<DiffResultContent> _updated = Enumerable.Empty<DiffResultContent>();
    private IEnumerable<DiffResultContent> _deleted = Enumerable.Empty<DiffResultContent>();
    private IEnumerable<Change> _changes = Enumerable.Empty<Change>();
    private DiffResultContent? _selectedResultContent;
    private CountStatusText _addStatusText = new CountStatusText(0, 0, 0);
    private CountStatusText _updateStatusText = new CountStatusText(0, 0, 0);
    private CountStatusText _deleteStatusText = new CountStatusText(0, 0, 0);
    public ResultWindowViewModel()
    {
        NextCommand = new NextCommand(this);
        PrevCommand = new PrevCommand(this);
    }

    public IEnumerable<DiffResultContent> Added
    {
        get => _added;
        set
        {
            SetProperty(ref _added, value);
        }
    }

    public IEnumerable<DiffResultContent> Updated
    {
        get => _updated;
        set
        {
            SetProperty(ref _updated, value);
        }
    }

    public IEnumerable<DiffResultContent> Deleted
    {
        get => _deleted;
        set
        {
            SetProperty(ref _deleted, value);
        }
    }

    public DiffResultContent? SelectedResultContent
    {
        get => _selectedResultContent;
        set
        {
            SetProperty(ref _selectedResultContent, value);
            if(_selectedResultContent == null)
            {
                Changes = Enumerable.Empty<Change>();
            }
            else
            {
                Changes = _selectedResultContent.Changes;
            }
        }
    }

    public IEnumerable<Change> Changes
    {
        get => _changes;
        set
        {
            SetProperty(ref _changes, value);
        }
    }

    public CountStatusText AddStatusText
    {
        get => _addStatusText;
        set
        {
            SetProperty(ref _addStatusText, value);
        }
    }

    public CountStatusText UpdateStatusText
    {
        get => _updateStatusText;
        set
        {
            SetProperty(ref _updateStatusText, value);
        }
    }

    public CountStatusText DeleteStatusText
    {
        get => _deleteStatusText;
        set
        {
            SetProperty(ref _deleteStatusText, value);
        }
    }

    public int AddedIndex { get; set; } = -1;
    public int UpdatedIndex { get; set; } = -1;
    public int DeletedIndex { get; set; } = -1;
    public IEnumerable<DiffResultContent> AddedRow { get; set; } = Enumerable.Empty<DiffResultContent>();
    public IEnumerable<DiffResultContent> UpdatedRow { get; set; } = Enumerable.Empty<DiffResultContent>();
    public IEnumerable<DiffResultContent> DeletedRow { get; set; } = Enumerable.Empty<DiffResultContent>();
    public ICommand NextCommand { get; }
    public ICommand PrevCommand { get; }
}
