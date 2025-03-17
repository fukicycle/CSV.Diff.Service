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
        return true;
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
                _viewModel.ColumnList = content.Header.ToImmutableList();
            }
        }
    }
}
