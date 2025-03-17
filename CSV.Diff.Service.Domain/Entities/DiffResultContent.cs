namespace CSV.Diff.Service.Domain.Entities;

    public sealed class DiffResultContent
    {
    public DiffResultContent(IReadOnlyCollection<IDictionary<string, string?>> values)
    {
        Values = values;
    }
    public IReadOnlyCollection<IDictionary<string, string?>> Values { get; }
}