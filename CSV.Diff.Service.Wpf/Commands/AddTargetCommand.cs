using System.Collections.Immutable;
using System.Windows.Input;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class AddTargetCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    public AddTargetCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _viewModel.ColumnList.HasValue && !_viewModel.IsRunning;
    }

    public void Execute(object? parameter)
    {
        var selected = _viewModel.ColumnList.Value.Where(a => a.IsSelected);
        if (selected.Any())
        {
            _viewModel.TargetColumnList = _viewModel.TargetColumnList.AddRange(selected.Where(a => !_viewModel.TargetColumnList.Contains(a.Item)).Select(a => a.Item));
        }
    }
}
