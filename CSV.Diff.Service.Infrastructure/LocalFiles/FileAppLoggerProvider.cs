using System.Text;
using CSV.Diff.Service.Domain.Interfaces;
namespace CSV.Diff.Service.Infrastructure.LocalFiles;
public sealed class FileAppLoggerProvider : IAppLoggerProvider
{
    private readonly string LOGGING_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "csv-diff.log");

    public void WriteLine(string message)
    {
        bool canSuccess = false;
        while (!canSuccess)
        {
            try
            {
                using var file = new FileStream(LOGGING_PATH, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                using var writer = new StreamWriter(file, Encoding.UTF8);
                writer.WriteLine(message);
                canSuccess = true;
            }
            catch (Exception)
            {
                //書き込みミスは握りつぶす。
            }
        }
    }
}