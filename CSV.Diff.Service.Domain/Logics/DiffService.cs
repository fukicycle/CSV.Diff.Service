using System.Collections.Immutable;
using CSV.Diff.Service.Domain.Entities;

namespace CSV.Diff.Service.Domain.Logics;

public static class DiffService
{
    public static Task<DiffResult> RunAsync(
        IReadOnlyCollection<IDictionary<string, string?>> prevData, 
        IReadOnlyCollection<IDictionary<string, string?>> afterData, 
        string baseKey,
        string[] targetColumns)
    {
        var tcs = new TaskCompletionSource<DiffResult>();
        //別スレッドで実施
        Task.Run(() =>
        {
            //指定されたカラムのみに絞り込む
            var prevDict = prevData.Select(a => 
                                        a.Where(kvp => targetColumns.Contains(kvp.Key)))
                                    .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                                    .ToList();
            var afterDict = afterData.Select(a =>
                                        a.Where(kvp => targetColumns.Contains(kvp.Key)))
                                      .Select(dict => dict.ToDictionary(kvp => kvp.Key,kvp => kvp.Value))
                                      .ToList();

            // 追加されたデータ（prevDict に存在しない current のデータ）
            var addedData = afterDict.AsParallel()
                                     .Where(after => 
                                            !prevDict.Any(prev => prev[baseKey] == after[baseKey]))
                                     .ToArray();

            // 削除されたデータ（currentDict に存在しない prev のデータ）
            var deletedData = prevDict.AsParallel()
                                      .Where(prev =>
                                            !afterDict.Any(current => current[baseKey] == prev[baseKey]))
                                      .ToArray();

            // 変更のあるデータ（ID は同じだが値が違う）
            var updatedData = afterDict.AsParallel()
                                       .Where(current =>
                                            prevDict.Any(prev => prev[baseKey] == current[baseKey] && !prev.SequenceEqual(current)))
                                       .ToArray();

            tcs.SetResult(new DiffResult(
                new DiffResultContent(addedData),
                new DiffResultContent(deletedData),
                new DiffResultContent(updatedData)));
        });
        return tcs.Task;
    }
}
