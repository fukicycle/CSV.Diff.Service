using System.Diagnostics;
using System.IO;
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
            var savePathAdd = await _resultWriter.WriteAsync("追加.csv", result.Added);
            var savePathDelete = await _resultWriter.WriteAsync("削除.csv", result.Deleted);
            var savePathUpdate = await _resultWriter.WriteAsync("更新.csv", result.Updated);
            var diffTime = DateTime.Now - startTime;
            _viewModel.StatusText = $"比較が終了しました。経過時間:{diffTime.Minutes}分{diffTime.Seconds}秒";
            MessageBox.Show("比較が終了しました。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
            var targetDir = new FileInfo(savePathAdd.Value).Directory?.FullName;
            var psi = new ProcessStartInfo();
            psi.FileName = targetDir;
            psi.UseShellExecute = true;
            psi.WorkingDirectory = targetDir;
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            _viewModel.StatusText = $"エラー:'{ex.Message}'";
            MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _viewModel.IsRunning = false;
        }
    }
}
