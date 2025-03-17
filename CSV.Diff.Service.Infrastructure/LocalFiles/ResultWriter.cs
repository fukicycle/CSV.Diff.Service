using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Helpers;
using CSV.Diff.Service.Domain.Interfaces;

namespace CSV.Diff.Service.Infrastructure.LocalFiles;

public sealed class ResultWriter : IResultWriter
{
    public async Task WriteAsync(string targetFileName, DiffResultContent content)
    {
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var targetDir = "csv-diff";
        var runDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var writeTargetDir = Path.Combine(baseDir, targetDir, runDateTime);
        if(!Directory.Exists(writeTargetDir))
        {
            Directory.CreateDirectory(writeTargetDir);
        }
        var writeTarget = Path.Combine(writeTargetDir, targetFileName);
        var data = content.Values.Select(a => string.Join(",", a.Keys.Select(v => v.ToCsvFormat()))).ToList();
        data.AddRange(content.Values.Select(a => string.Join(",", a.Values.Select(v => v.ToCsvFormat()))));
        await File.WriteAllLinesAsync(writeTarget, data);
    }
}
