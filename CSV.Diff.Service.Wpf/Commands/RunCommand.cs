using System.Windows;
using System.Windows.Input;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.Logics;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class RunCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    private readonly ResultWindowViewModel _resultWindowViewModel;
    // private readonly IResultWriter _resultWriter;
    private readonly IDiffService _diffService;
    private readonly IAppLogger _logger;
    public RunCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _resultWindowViewModel = (ResultWindowViewModel)DI.Provider.GetService(typeof(ResultWindowViewModel))!; ;
        // _resultWriter = (IResultWriter)DI.Provider.GetService(typeof(IResultWriter))!;
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
            _resultWindowViewModel.AddedRow = result.Added;
            _resultWindowViewModel.UpdatedRow = result.Updated;
            _resultWindowViewModel.DeletedRow = result.Deleted;
            _resultWindowViewModel.NextCommand.Execute(NextCommand.ADDED);
            _resultWindowViewModel.NextCommand.Execute(NextCommand.UPDATED);
            _resultWindowViewModel.NextCommand.Execute(NextCommand.DELETED);
            //var savePathAdd = await _resultWriter.WriteAsync("追加.csv", result.Added);
            //var savePathDelete = await _resultWriter.WriteAsync("削除.csv", result.Deleted);
            //var savePathUpdate = await _resultWriter.WriteAsync("更新.csv", result.Updated);
            var diffTime = DateTime.Now - startTime;
            _viewModel.StatusText = $"比較が終了しました。経過時間:{diffTime.Minutes}分{diffTime.Seconds}秒";
            new ResultWindow().Show();
            // MessageBox.Show("比較が終了しました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
            // var targetDir = new FileInfo(savePathAdd.Value).Directory?.FullName;
            // var psi = new ProcessStartInfo();
            // psi.FileName = targetDir;
            // psi.UseShellExecute = true;
            // psi.WorkingDirectory = targetDir;
            // Process.Start(psi);
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
