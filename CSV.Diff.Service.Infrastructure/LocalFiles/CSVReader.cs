using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.ValueObjects;
using Microsoft.VisualBasic.FileIO;

namespace CSV.Diff.Service.Infrastructure.LocalFiles;

public sealed class CSVReader : ICSVReader
{
    private readonly IAppLogger _logger;
    public CSVReader(IAppLogger logger)
    {
        _logger = logger;
    }

    public Task<CSVContent> ReadAsync(FilePath filePath)
    {
        var tcs = new TaskCompletionSource<CSVContent>();
        var isFirstRow = true;
        Task.Run(() =>
        {
            try
            {
                _logger.LogInformation($"{filePath.ShortName}を読み取ります。");
                string[] header = Array.Empty<string>();
                IEnumerable<string[]> contents = Enumerable.Empty<string[]>();
                using var stream = new FileStream(filePath.Value, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var parser = new TextFieldParser(stream);
                parser.Delimiters = [","];
                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();
                    if (fields == null)
                    {
                        continue;
                    }
                    _logger.LogDebug($"行:{string.Join(",", fields)}");
                    if (isFirstRow)
                    {
                        header = fields;
                        isFirstRow = false;
                    }
                    else
                    {
                        contents = contents.Append(fields);
                    }
                }
                tcs.SetResult(new CSVContent(header, contents));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }
}
