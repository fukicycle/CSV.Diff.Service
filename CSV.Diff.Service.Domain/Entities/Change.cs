namespace CSV.Diff.Service.Domain.Entities;

public sealed class Change
{
    public Change(
        string column,
        string? prev,
        string? after)
    {
        Column = column;
        Prev = prev;
        After = after;
    }
    public string Column { get; }
    public string? Prev { get; }
    public string? After { get; }
}
