using System.Windows;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.Service.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ResultWindow : Window
{
    public ResultWindow()
    {
        InitializeComponent();
        DataContext = (ResultWindowViewModel)DI.Provider.GetService(typeof(ResultWindowViewModel))!;
    }
}