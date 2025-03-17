using System.Windows;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Infrastructure.LocalFiles;
using Microsoft.Extensions.DependencyInjection;

namespace CSV.Diff.Service.Wpf;

internal static class DI
{
    public static IServiceProvider Provider { get; }
    static DI()
    {
        Provider = CreateServiceProvider();
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        try
        {
            var appSettings = new AppSettings();
            services.AddSingleton(appSettings);
            services.AddSingleton<ICSVReader, CSVReader>();
            services.AddSingleton<IResultWriter, ResultWriter>();
        }
        catch (Exception ex)
        {
            if (MessageBox.Show(ex.Message, "設定エラー", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                MessageBox.Show("CSV比較ツールを終了します。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
        }
        return services.BuildServiceProvider();
    }
}