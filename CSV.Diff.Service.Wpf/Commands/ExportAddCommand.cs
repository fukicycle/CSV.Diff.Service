using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Logics;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf.Commands;

public sealed class ExportAddCommand : ICommand
{
    private readonly ResultWindowViewModel _viewModel;
    public event EventHandler? CanExecuteChanged;

    public ExportAddCommand(ResultWindowViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        string content = string.Join(Environment.NewLine, _viewModel.AddedRow.Select(a => a.RawContent.ToCsv()));
        try
        {
            string path = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                 "csv比較ツール",
                                  _viewModel.ResultCreationDateTime.ToString("yyyy-MM-dd_HHmmss"));
            string name = "追加.csv";
            var savedFile = new SavedFile(path, name);
            savedFile.Write(content);
            MessageBox.Show("保存しました。");
            savedFile.Open();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
