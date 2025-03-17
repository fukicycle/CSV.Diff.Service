namespace CSV.Diff.Service.Domain.Entities;

public sealed class SelectionItem<T>
{
    public SelectionItem(T item)
    {
        Item = item;
    }
    public T Item { get; }
    public bool IsSelected { get; set;}
}
