using CSV.Diff.Service.Domain.Exceptions;

namespace CSV.Diff.Service.Domain.ValueObjects;

public sealed class FilePath : ValueObject<FilePath>
{
    public static readonly FilePath Empty = new FilePath();
    private FilePath()
    {
        Value = string.Empty;
        ShortName = string.Empty;
    }

    public FilePath(string value)
    {
        if (File.Exists(value))
        {
            var fileInfo = new FileInfo(value);
            Value = fileInfo.FullName;
            ShortName = fileInfo.Name;
        }
        else
        {
            throw new FilePathException($"Specified file is not found. ({value})");
        }
    }
    public string Value { get; }
    public string ShortName { get; }
    protected override bool EqualsCore(FilePath other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}
