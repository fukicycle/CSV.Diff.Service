namespace CSV.Diff.Service.Domain.Entities;

public sealed class DiffResult
{
    public DiffResult(
        string[] header,
        IEnumerable<DiffResultContent> added,
        IEnumerable<DiffResultContent> deleted,
        IEnumerable<DiffResultContent> updated)
    {
        Header = header;
        Added = added;
        Deleted = deleted;
        Updated = updated;
    }
    public string[] Header { get; }
    public IEnumerable<DiffResultContent> Added { get; }
    public IEnumerable<DiffResultContent> Deleted { get; }
    public IEnumerable<DiffResultContent> Updated { get; }
}