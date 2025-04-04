using System.Windows.Input;
using CSV.Diff.Service.Domain.Helpers;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class NextCommand : ICommand
{
    public const int ADDED = 1;
    public const int UPDATED = 2;
    public const int DELETED = 3;
    public const int ROW_COUNT = 100;
    private readonly ResultWindowViewModel _viewModel;

    public NextCommand(ResultWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        switch (int.Parse(parameter?.ToString() ?? "0"))
        {
            case ADDED:
                return Math.Min((_viewModel.AddedIndex + 1) * ROW_COUNT, _viewModel.AddedRow.Count()) != _viewModel.AddedRow.Count();
            case UPDATED:
                return Math.Min((_viewModel.UpdatedIndex + 1) * ROW_COUNT, _viewModel.UpdatedRow.Count()) != _viewModel.UpdatedRow.Count();
            case DELETED:
                return Math.Min((_viewModel.DeletedIndex + 1) * ROW_COUNT, _viewModel.DeletedRow.Count()) != _viewModel.DeletedRow.Count();
            default:
                throw new ArgumentException("You have to set parameter.");
        }
    }

    public void Execute(object? parameter)
    {
        switch (int.Parse(parameter?.ToString() ?? "0"))
        {
            case ADDED:
                _viewModel.AddedIndex = _viewModel.AddedIndex + 1;
                _viewModel.Added = _viewModel.AddedRow.GetCalculateRange(_viewModel.AddedIndex, ROW_COUNT);
                _viewModel.AddStatusText = new CountStatusText(_viewModel.AddedIndex, ROW_COUNT, _viewModel.AddedRow.Count());
                break;
            case UPDATED:
                _viewModel.UpdatedIndex = _viewModel.UpdatedIndex + 1;
                _viewModel.Updated = _viewModel.UpdatedRow.GetCalculateRange(_viewModel.UpdatedIndex, ROW_COUNT);
                _viewModel.UpdateStatusText = new CountStatusText(_viewModel.UpdatedIndex, ROW_COUNT, _viewModel.UpdatedRow.Count());
                break;
            case DELETED:
                _viewModel.DeletedIndex = _viewModel.DeletedIndex + 1;
                _viewModel.Deleted = _viewModel.DeletedRow.GetCalculateRange(_viewModel.DeletedIndex, ROW_COUNT);
                _viewModel.DeleteStatusText = new CountStatusText(_viewModel.DeletedIndex, ROW_COUNT, _viewModel.DeletedRow.Count());
                break;
            default:
                throw new ArgumentException("You have to set parameter.");
        }
    }
}
