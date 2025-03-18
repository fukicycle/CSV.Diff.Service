using CSV.Diff.Service.Domain.Entities;
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
        string baseKey,
        IEnumerable<string> targetColumns)
    {
        var tcs = new TaskCompletionSource<DiffResult>();
        _logger.LogInformation($"データを絞り込みます。列:{string.Join(",", targetColumns)}");

        Task.Run(() =>
        {
            //指定されたカラムのみに絞り込む
            var prevDict = prevData.Select(a =>
                                        a.Where(kvp => targetColumns.Contains(kvp.Key)))
                                    .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                                    .ToList();
            var afterDict = afterData.Select(a =>
                                        a.Where(kvp => targetColumns.Contains(kvp.Key)))
                                      .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                                      .ToList();

            _logger.LogInformation($"鍵のみに絞り込みます。");
            // prevDict と afterDict の baseKey の値を HashSet に格納
            var prevKeys = new HashSet<string?>(prevDict.Select(d => d.ContainsKey(baseKey) ? d[baseKey] : null));
            var afterKeys = new HashSet<string?>(afterDict.Select(d => d.ContainsKey(baseKey) ? d[baseKey] : null));

            _logger.LogInformation($"追加されたデータを検索します。");
            // 追加されたデータ（prevDict に存在しない current のデータ）
            var addedData = afterDict.AsParallel()
                                     .Where(after => !prevKeys.Contains(after.ContainsKey(baseKey) ? after[baseKey] : null))
                                     .ToArray();
            _logger.LogInformation($"追加されたデータを検索しました。件数:{addedData.Length}");

            _logger.LogInformation($"削除されたデータを検索します。");
            // 削除されたデータ（currentDict に存在しない prev のデータ）
            var deletedData = prevDict.AsParallel()
                                      .Where(prev => !afterKeys.Contains(prev.ContainsKey(baseKey) ? prev[baseKey] : null))
                                      .ToArray();
            _logger.LogInformation($"削除されたデータを検索しました。件数:{deletedData.Length}");

            _logger.LogInformation($"更新されたデータを検索します。");
            // 変更のあるデータ（ID は同じだが値が違う）
            var updatedData = afterDict.AsParallel()
                                       .Where(current =>
                                               prevKeys.Contains(current.ContainsKey(baseKey) ? current[baseKey] : null) &&
                                               prevDict.Any(prev =>
                                                  prev.ContainsKey(baseKey) &&
                                                  prev[baseKey] == current[baseKey] &&
                                                  !prev.SequenceEqual(current)))
                                       .ToArray();
            _logger.LogInformation($"更新されたデータを検索しました。件数:{updatedData.Length}");

            tcs.SetResult(new DiffResult(
                new DiffResultContent(addedData),
                new DiffResultContent(deletedData),
                new DiffResultContent(updatedData)));
        });
        return tcs.Task;
    }
}
