using System.Windows;
using System.Windows.Input;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.Logics;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class RunCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    private readonly IResultWriter _resultWriter;
    public RunCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _resultWriter = (IResultWriter)DI.Provider.GetService(typeof(IResultWriter))!;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _viewModel.TargetColumnList.Any() && 
                !_viewModel.IsRunning &&
                _viewModel.TargetColumnList.Contains(_viewModel.KeyColumn);
    }

    public async void Execute(object? parameter)
    {
        _viewModel.IsRunning = true;
        try
        {
            var result = await DiffService.RunAsync(
                                _viewModel.PreviousData.Raw,
                                _viewModel.AfterData.Raw,
                                _viewModel.KeyColumn,
                                _viewModel.TargetColumnList);
            await _resultWriter.WriteAsync("Added", result.Added);
            await _resultWriter.WriteAsync("Deleted", result.Deleted);
            await _resultWriter.WriteAsync("Updated", result.Updated);
            MessageBox.Show("比較が終了しました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.IsRunning = false;
        }
    }
}
