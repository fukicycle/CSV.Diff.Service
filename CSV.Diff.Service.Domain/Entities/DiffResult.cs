using System;

namespace CSV.Diff.Service.Domain.Entities;

public sealed class DiffResult
{
    public DiffResult(
        DiffResultContent added,
        DiffResultContent deleted,
        DiffResultContent updated)
    {
        Added = added;
        Deleted = deleted;
        Updated = updated;
    }

    public DiffResultContent Added { get; }
    public DiffResultContent Deleted { get; }
    public DiffResultContent Updated { get; }
}