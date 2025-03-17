using System.Collections.Immutable;
using CSV.Diff.Service.Domain.Entities;

namespace CSV.Diff.Service.Domain.ValueObjects;

public sealed class ColumnList : ValueObject<ColumnList>
{
    public ColumnList(IEnumerable<string> value)
    {
        Value = value.Select(a => new SelectionItem<string>(a)).ToImmutableList();
        HasValue = Value.Any();
    }
    public ImmutableList<SelectionItem<string>> Value { get; }
    public bool HasValue { get; }
    protected override bool EqualsCore(ColumnList other)
    {
        return Value.SequenceEqual(other.Value);
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}
