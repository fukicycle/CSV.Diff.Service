using System.Collections.Immutable;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Helpers;
using CSV.Diff.Service.Domain.Interfaces;

namespace CSV.Diff.Service.Domain.Logics;

public sealed class DiffService : IDiffService
{
    private readonly IAppLogger _logger;
    public DiffService(IAppLogger logger)
    {
        _logger = logger;
    }

    public Task<DiffResult> RunAsync(
        IReadOnlyCollection<IDictionary<string, string?>> prevData,
        IReadOnlyCollection<IDictionary<string, string?>> afterData,
        string baseKey,
        IEnumerable<string> targetColumns)
    {
        var tcs = new TaskCompletionSource<DiffResult>();
        _logger.LogWarning("DiffService is not available. You have to use DiffServiceV2");
        tcs.SetException(new Exception("DiffService is not available. You have to use DiffServiceV2"));
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
                                            !prevDict.Any(prev => prev[baseKey] == after[baseKey]))
                                     .ToArray();
            _logger.LogInformation($"追加されたデータを検索しました。件数:{addedData.Length}");

            _logger.LogInformation($"削除されたデータを検索します。");
            // 削除されたデータ（currentDict に存在しない prev のデータ）
            var deletedData = prevDict.AsParallel()
                                      .Where(prev =>
                                            !afterDict.Any(current => current[baseKey] == prev[baseKey]))
                                      .ToArray();
            _logger.LogInformation($"削除されたデータを検索しました。件数:{deletedData.Length}");

            _logger.LogInformation($"更新されたデータを検索します。");
            // 変更のあるデータ（ID は同じだが値が違う）
            var updatedData = afterDict.AsParallel()
                                       .Where(current =>
                                            prevDict.Any(prev => prev[baseKey] == current[baseKey] && !prev.EqualAllProperty(current, current[baseKey], _logger)))
                                       .ToArray();
            _logger.LogInformation($"更新されたデータを検索しました。件数:{updatedData.Length}");

            // tcs.SetResult(new DiffResult(
            //     targetColumns.ToArray(),
            //     addedData,
            //     deletedData,
            //     updatedData));
        });
        return tcs.Task;
    }
}
