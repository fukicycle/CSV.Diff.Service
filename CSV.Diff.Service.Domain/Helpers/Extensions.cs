using System.Data;
using System.Net.Http.Headers;
using CSV.Diff.Service.Domain.Entities;

namespace CSV.Diff.Service.Domain.Helpers;

public static class Extensions
{
    public static IReadOnlyCollection<IDictionary<string,string?>> ToDictionary(this CSVContent content)
    {
        return content.Contents
                      .Select(columns => 
                                    content.Header
                                           .Zip(columns, 
                                                (header, value) => 
                                                    new { header, value })
                                            .ToDictionary(a => a.header, a => a.value))
                                            .ToList()
                                            .AsReadOnly();
    }

    public static string? ToCsvFormat(this string? cell)
    {
        if(cell == null)
        {
            return null;
        }
        if(cell.All(a => char.IsNumber(a)))
        {
            return cell;
        }
        return $"\"{cell.Replace("\"", "\"\"")}\"";
    }
}
