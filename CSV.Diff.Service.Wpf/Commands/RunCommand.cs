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
        _viewModel.StatusText = "CSVファイルを比較しています。";
        var startTime = DateTime.Now;
        try
        {
            var result = await DiffService.RunAsync(
                                _viewModel.PreviousData.Raw,
                                _viewModel.AfterData.Raw,
                                _viewModel.KeyColumn,
                                _viewModel.TargetColumnList);
            await _resultWriter.WriteAsync("Added.csv", result.Added);
            await _resultWriter.WriteAsync("Deleted.csv", result.Deleted);
            await _resultWriter.WriteAsync("Updated.csv", result.Updated);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            var diffTime = DateTime.Now - startTime;
            _viewModel.StatusText = $"比較が終了しました。経過時間:{diffTime.Minutes}分{diffTime.Seconds}秒";
            _viewModel.IsRunning = false;
            MessageBox.Show("比較が終了しました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
