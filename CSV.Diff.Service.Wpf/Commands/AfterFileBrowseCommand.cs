using System.Windows.Input;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Wpf.ViewModels;
using Microsoft.Win32;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class AfterFileBrowseCommand : ICommand
{
    private readonly MainWindowViewModel _viewModel;

    public AfterFileBrowseCommand(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var ofd = new OpenFileDialog { Filter = "*.csv|*.csv", Multiselect = false };
        if(ofd.ShowDialog() == true)
        {
            _viewModel.AfterFile = new FilePath(ofd.FileName);
        }
    }
}
