using CSV.Diff.Service.Domain.Exceptions;

namespace CSV.Diff.Service.Domain.ValueObjects;

public sealed class FilePath : ValueObject<FilePath>
{
    public static readonly FilePath Empty = new FilePath();
    private FilePath()
    {
        Value = string.Empty;
    }

    public FilePath(string value)
    {
        if (File.Exists(value))
        {
            Value = value;
        }
        else
        {
            throw new FilePathException($"Specified file is not found. ({value})");
        }
    }
    public string Value { get; }
    protected override bool EqualsCore(FilePath other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}
