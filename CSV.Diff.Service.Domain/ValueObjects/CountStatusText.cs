namespace CSV.Diff.Service.Domain.ValueObjects;

public sealed class CountStatusText : ValueObject<CountStatusText>
{
    public CountStatusText(int index, int numberOfDisplay, int maxSize)
    {
        int end = Math.Min((index + 1) * numberOfDisplay, maxSize);
        Value = $"{maxSize}件中{index * numberOfDisplay + 1}件 ~ {end}件を表示";
    }
    public string Value { get; }
    protected override bool EqualsCore(CountStatusText other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}
