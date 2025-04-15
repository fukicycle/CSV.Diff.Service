using CSV.Diff.Service.Domain.ValueObjects;

namespace CSV.Diff.Service.Domain.Entities;

public sealed class SavedFile : ValueObject<SavedFile>
{
    public SavedFile(string path, string filename)
    {
        Directory = path;
        Fullname = Path.Combine(path, filename);
    }
    public string Fullname { get; }
    public string Directory { get; }
    protected override bool EqualsCore(SavedFile other)
    {
        return Fullname == other.Fullname;
    }

    protected override int GetHashCodeCore()
    {
        return Fullname.GetHashCode();
    }
}
