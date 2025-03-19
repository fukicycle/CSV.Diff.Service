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
                var messageWithNewLine = message + Environment.NewLine;
                File.AppendAllText(LOGGING_PATH, messageWithNewLine);
                canSuccess = true;
            }
            catch (Exception)
            {
                //書き込みミスは握りつぶす。
            }
        }
    }
}