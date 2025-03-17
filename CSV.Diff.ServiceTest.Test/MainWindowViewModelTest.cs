using System.Collections.Immutable;
using CSV.Diff.Service.Domain.ValueObjects;
using CSV.Diff.Service.Wpf.ViewModels;

namespace CSV.Diff.ServiceTest.Test;

public class MainWindowViewModelTest
{
    private MainWindowViewModel _viewModel;
    [SetUp]
    public void SetUp()
    {
        _viewModel = new MainWindowViewModel();
    }

    [Test]
    public void Initial()
    {
        _viewModel.PreviousFile.Is(FilePath.Empty);
        _viewModel.AfterFile.Is(FilePath.Empty);
        _viewModel.PreviousData.Is(PreviewData.Empty);
        _viewModel.AfterData.Is(PreviewData.Empty);
        _viewModel.ColumnList.Is(ImmutableList<string>.Empty);
        _viewModel.TargetColumnList.Is(ImmutableList<string>.Empty);
    }
}
