using System.Text;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Helpers;
using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.ValueObjects;

namespace CSV.Diff.Service.Infrastructure.LocalFiles;

public sealed class ResultWriter : IResultWriter
{
    private readonly IAppLogger _logger;
    public ResultWriter(IAppLogger logger)
    {
        _logger = logger;
    }

    public async Task<FilePath> WriteAsync(string targetFileName, DiffResultContent content)
    {
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var targetDir = "csv-diff";
        var runDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var writeTargetDir = Path.Combine(baseDir, targetDir, runDateTime);
        if (!Directory.Exists(writeTargetDir))
        {
            _logger.LogInformation($"{writeTargetDir}フォルダを作成します。");
            Directory.CreateDirectory(writeTargetDir);
        }
        var writeTarget = Path.Combine(writeTargetDir, targetFileName);
        var data = content.Values.Select(a => string.Join(",", a.Keys.Select(v => v.ToCsvFormat()))).Distinct().ToList();
        data.AddRange(content.Values.Select(a => string.Join(",", a.Values.Select(v => v.ToCsvFormat()))));
        var provider = CodePagesEncodingProvider.Instance;
        await File.WriteAllLinesAsync(writeTarget, data, provider.GetEncoding("shift_jis") ?? Encoding.UTF8);
        _logger.LogInformation($"{targetFileName}に保存しました。");
        return new FilePath(writeTarget);
    }
}
