using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.ValueObjects;
using Microsoft.VisualBasic.FileIO;

namespace CSV.Diff.Service.Infrastructure.LocalFiles;

public sealed class CSVReader : ICSVReader
{
    public Task<CSVContent> ReadAsync(FilePath filePath)
    {
        var tcs = new TaskCompletionSource<CSVContent>();
        var isFirstRow = true;
        Task.Run(() =>
        {
            try
            {

                string[] header = Array.Empty<string>();
                IEnumerable<string[]> contents = Enumerable.Empty<string[]>();
                using var stream = new FileStream(filePath.Value, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var parser = new TextFieldParser(stream);
                parser.Delimiters = [","];
                while (!parser.EndOfData)
                {
                    var fileds = parser.ReadFields();
                    if (fileds == null)
                    {
                        continue;
                    }
                    if (isFirstRow)
                    {
                        header = fileds;
                        isFirstRow = false;
                    }
                    else
                    {
                        contents = contents.Append(fileds);
                    }
                }
                tcs.SetResult(new CSVContent(header, contents));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }
}
