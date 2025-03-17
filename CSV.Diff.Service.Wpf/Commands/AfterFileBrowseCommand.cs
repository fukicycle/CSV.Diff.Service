using System.Collections.Immutable;
using System.Windows.Input;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Wpf.ViewModels;
using Microsoft.Win32;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class AfterFileBrowseCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;
    private readonly ICSVReader _reader;

    public AfterFileBrowseCommand(MainWindowViewModel viewModel)
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
        if(ofd.ShowDialog() == true)
        {
            _viewModel.AfterFile = new FilePath(ofd.FileName);
            var content = await _reader.ReadAsync(_viewModel.AfterFile);
            _viewModel.AfterData = new PreviewData(content);
            if(_viewModel.PreviousData.Equals(_viewModel.AfterData))
            {
                _viewModel.ColumnList = content.Header.ToImmutableList();
            }
        }
    }
}
