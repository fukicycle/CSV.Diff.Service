using CSV.Diff.Service.Domain.Entities;

namespace CSV.Diff.Service.Domain.Logics;

public interface IDiffService
{
    Task<DiffResult> RunAsync(
            IReadOnlyCollection<IDictionary<string, string?>> prevData,
            IReadOnlyCollection<IDictionary<string, string?>> afterData,
            string baseKey,
            IEnumerable<string> targetColumns);
}
