using System.Windows.Input;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Domain.Helpers;
using CSV.Diff.Service.Wpf.ViewModels;
using Microsoft.Win32;
using System.Collections.Immutable;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class PreviousFileBrowseCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    private readonly ICSVReader _reader;

    public PreviousFileBrowseCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _reader = (ICSVReader)DI.Provider.GetService(typeof(ICSVReader))!;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        if (_viewModel.IsRunning)
        {
            return false;
        }
        else
        {
            if (_viewModel.PreviousData.Equals(PreviewData.Empty))
            {
                return true;
            }
            if (_viewModel.PreviousData.Equals(_viewModel.AfterData))
            {
                return false;
            }
            else
            {
                if(_viewModel.AfterData.Equals(PreviewData.Empty))
                {
                    return false;
                }
                return true;
            }
        }
    }

    public async void Execute(object? parameter)
    {
        var ofd = new OpenFileDialog { Filter = "*.csv|*.csv", Multiselect = false };
        if (ofd.ShowDialog() == true)
        {
            _viewModel.PreviousFile = new FilePath(ofd.FileName);
            var content = await _reader.ReadAsync(_viewModel.PreviousFile);
            _viewModel.PreviousData = new PreviewData(content);
            if (_viewModel.PreviousData.Equals(_viewModel.AfterData))
            {
                _viewModel.ColumnList = new ColumnList(content.Header);
                _viewModel.StatusText = "*が表示されているパラメータを入力して実行を押してください。";
            }
            else
            {
                _viewModel.StatusText = "CSVファイルに含まれるヘッダーが一致しません。";
            }
        }
    }
}
