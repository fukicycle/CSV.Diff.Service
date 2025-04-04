namespace CSV.Diff.Service.Domain.Entities;

public sealed class DiffResultContent
{
    public DiffResultContent(
        string?[] raw,
        string? baseValue,
        IEnumerable<Change> changes)
    {
        BaseValue = baseValue ?? throw new Exception("基準となる列の値が入っていません。");
        RawContent = string.Join(",", raw);
        Changes = changes.ToList();
    }
    public string BaseValue { get; }
    public string RawContent { get; }
    public IReadOnlyCollection<Change> Changes { get; }
}