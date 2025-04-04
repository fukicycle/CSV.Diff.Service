using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Helpers;
using CSV.Diff.Service.Domain.Interfaces;

namespace CSV.Diff.Service.Domain.Logics;

public sealed class DiffServiceV2 : IDiffService
{
    private readonly IAppLogger _logger;
    public DiffServiceV2(IAppLogger logger)
    {
        _logger = logger;
    }

    public Task<DiffResult> RunAsync(
        IReadOnlyCollection<IDictionary<string, string?>> prevData,
        IReadOnlyCollection<IDictionary<string, string?>> afterData,
        string baseColumn,
        IEnumerable<string> targetColumns)
    {
        var tcs = new TaskCompletionSource<DiffResult>();
        //別スレッドで実施
        Task.Run(() =>
        {
            _logger.LogInformation($"データを絞り込みます。列:{string.Join(",", targetColumns)}");
            //指定されたカラムのみに絞り込む
            var prevDict = prevData.Select(a =>
                                        a.Where(kvp => targetColumns.Contains(kvp.Key)))
                                    .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                                    .ToList();
            var afterDict = afterData.Select(a =>
                                        a.Where(kvp => targetColumns.Contains(kvp.Key)))
                                      .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                                      .ToList();

            _logger.LogInformation($"追加されたデータを検索します。");
            // 追加されたデータ（prevDict に存在しない current のデータ）
            var addedData = afterDict.AsParallel()
                                     .Where(after =>
                                            !prevDict.Any(prev => prev[baseColumn] == after[baseColumn]))
                                     .Select(after =>
                                            new DiffResultContent(
                                                after.Values.ToArray(),
                                                after[baseColumn],
                                                Enumerable.Empty<Change>()))
                                     .ToArray();
            _logger.LogInformation($"追加されたデータを検索しました。件数:{addedData.Length}");

            _logger.LogInformation($"削除されたデータを検索します。");
            // 削除されたデータ（currentDict に存在しない prev のデータ）
            var deletedData = prevDict.AsParallel()
                                      .Where(prev =>
                                            !afterDict.Any(current => current[baseColumn] == prev[baseColumn]))
                                      .Select(prev =>
                                            new DiffResultContent(
                                                prev.Values.ToArray(),
                                                prev[baseColumn],
                                                Enumerable.Empty<Change>()))
                                      .ToArray();
            _logger.LogInformation($"削除されたデータを検索しました。件数:{deletedData.Length}");

            _logger.LogInformation($"更新されたデータを検索します。");
            // 変更のあるデータ（ID は同じだが値が違う）
            // var updatedData = afterDict.AsParallel()
            //                            .Where(current =>
            //                                 prevDict.Any(prev => prev[baseColumn] == current[baseColumn] && !prev.EqualAllProperty(current, current[baseColumn], _logger)))
            //                            .ToArray();

            var updatedData = new List<DiffResultContent>();
            prevDict.AsParallel().ForAll(prevValue => 
            {
                var baseValue = prevValue[baseColumn] ?? throw new Exception("基準となる列の値が入っていません。");
                var afterValue = afterDict.FirstOrDefault(after => after[baseColumn] == baseValue);
                if (afterValue != default)
                {
                    var changes = new List<Change>();
                    foreach (var column in prevValue.Keys)
                    {
                        var prevContent = prevValue[column];
                        var afterContent = afterValue[column];
                        if(prevContent != afterContent)
                        {
                            changes.Add(new Change(column, prevContent, afterContent));
                            _logger.LogDebug($"Change!{baseValue},{prevContent},{afterContent}");
                        }
                    }
                    if(changes.Any())
                    {
                        updatedData.Add(new DiffResultContent(afterValue.Values.ToArray(), baseValue, changes));
                    }
                }
            });
            _logger.LogInformation($"更新されたデータを検索しました。件数:{updatedData.Count}");

            tcs.SetResult(new DiffResult(
                targetColumns.ToArray(),
                addedData,
                deletedData,
                updatedData));
        });
        return tcs.Task;
    }
}
