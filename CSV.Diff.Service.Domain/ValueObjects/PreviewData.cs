namespace CSV.Diff.Service.Domain.ValueObjects;

public sealed class PreviewData : ValueObject<PreviewData>
{
    public static readonly PreviewData Empty = new PreviewData();
    private PreviewData()
    {
        
    }

    protected override bool EqualsCore(PreviewData other)
    {
        throw new NotImplementedException();
    }

    protected override int GetHashCodeCore()
    {
        throw new NotImplementedException();
    }
}
