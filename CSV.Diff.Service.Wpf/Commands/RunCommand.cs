using System.Windows;
using System.Windows.Input;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.Logics;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class RunCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    private readonly IDiffService _diffService;
    private readonly IAppLogger _logger;
    public RunCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _diffService = (IDiffService)DI.Provider.GetService(typeof(IDiffService))!;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
        _logger = (IAppLogger)DI.Provider.GetService(typeof(IAppLogger))!;
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
            var result = await _diffService.RunAsync(
                                _viewModel.PreviousData.Raw,
                                _viewModel.AfterData.Raw,
                                _viewModel.KeyColumn,
                                _viewModel.TargetColumnList);
            var resultWindowViewModel = new ResultWindowViewModel(result);
            resultWindowViewModel.NextCommand.Execute(NextCommand.ADDED);
            resultWindowViewModel.NextCommand.Execute(NextCommand.UPDATED);
            resultWindowViewModel.NextCommand.Execute(NextCommand.DELETED);
            var diffTime = DateTime.Now - startTime;
            _viewModel.StatusText = $"比較が終了しました。経過時間:{diffTime.Minutes}分{diffTime.Seconds}秒";
            new ResultWindow(resultWindowViewModel).Show();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace ?? "Stack Trace is Empty.");
            _viewModel.StatusText = $"エラー:'{ex.Message}'";
            MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.IsRunning = false;
        }
    }
}
