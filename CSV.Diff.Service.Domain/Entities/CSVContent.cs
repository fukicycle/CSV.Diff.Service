using System.Collections.Immutable;

namespace CSV.Diff.Service.Domain.Entities;

public sealed class CSVContent
{
    public CSVContent(
        string[] header,
        IEnumerable<string?[]> contents)
    {
        Header = header;
        var builder = ImmutableList.CreateBuilder<string?[]>();
        builder.AddRange(contents);
        Contents = builder.ToImmutableList();
    }
    public string[] Header { get; }
    public ImmutableList<string?[]> Contents { get; }
}
