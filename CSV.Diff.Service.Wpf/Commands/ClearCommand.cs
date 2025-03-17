using System.Windows.Input;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class ClearCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    public ClearCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _viewModel.TargetColumnList.Any();
    }

    public void Execute(object? parameter)
    {
        _viewModel.TargetColumnList = _viewModel.TargetColumnList.Clear();
    }
}
