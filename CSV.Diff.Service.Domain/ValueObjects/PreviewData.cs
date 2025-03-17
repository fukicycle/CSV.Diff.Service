using System.Data;
using System.Reflection.Metadata.Ecma335;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Helpers;

namespace CSV.Diff.Service.Domain.ValueObjects;

public sealed class PreviewData : ValueObject<PreviewData>
{
    public const int MAX_ROW_COUNT = 100;
    public static readonly PreviewData Empty = new PreviewData();
    private PreviewData()
    {
        Content = new CSVContent(Array.Empty<string>(), Enumerable.Empty<string[]>());
        Raw = new List<IDictionary<string, string?>>();
        Value = new DataTable();
    }

    public PreviewData(CSVContent content)
    {
        Content = content;
        Raw = content.ToDictionary();
        Value = CreateDataTable(content);
    }

    public DataTable Value { get; }
    private CSVContent Content { get; }
    public IReadOnlyCollection<IDictionary<string, string?>> Raw { get; }

    protected override bool EqualsCore(PreviewData other)
    {
        return Content.Header.SequenceEqual(other.Content.Header);
    }

    protected override int GetHashCodeCore()
    {
        return Content.Header.GetHashCode();
    }

    private DataTable CreateDataTable(CSVContent content)
    {
        var counter = 1;
        var table = new DataTable();
        // 動的カラム追加
        foreach (var header in content.Header)
        {
            table.Columns.Add(header);
        }

        var dictionary = content.ToDictionary();

        // データを `DataTable` に追加
        foreach (var rowItem in dictionary)
        {
            if(counter > MAX_ROW_COUNT)
            {
                break;
            }
            var row = table.NewRow();
            foreach (var colItem in rowItem)
            {
                row[colItem.Key] = colItem.Value;
            }
            table.Rows.Add(row);
            counter++;
        }
        return table;
    }
}
