using System.Data;
using System.Net.Http.Headers;
using System.Text;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.Interfaces;

namespace CSV.Diff.Service.Domain.Helpers;

public static class Extensions
{
    public static IReadOnlyCollection<IDictionary<string, string?>> ToDictionary(this CSVContent content)
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
        if (cell == null)
        {
            return null;
        }
        if (cell.All(a => char.IsNumber(a)))
        {
            return cell;
        }
        return $"\"{cell.Replace("\"", "\"\"")}\"";
    }

    public static bool EqualAllProperty(this Dictionary<string, string?> item1, Dictionary<string, string?> item2, string? baseKey, IAppLogger? logger = null)
    {
        if (!item1.Keys.SequenceEqual(item2.Keys))
        {
            logger?.LogError($"データに含まれるKeyが一致しません。Item1:{item1.Keys.Count},Item2:{item2.Keys.Count}");
            return false;
        }
        bool result = true;
        var changes = new List<string>();
        foreach (var key in item1.Keys)
        {
            var content1 = item1[key];
            var content2 = item2[key];
            if (content1 != content2)
            {
                string prev = string.Empty;
                if (string.IsNullOrEmpty(content1))
                {
                    prev = "値がありません";
                }
                else
                {
                    prev = content1;
                }
                string after = string.Empty;
                if (string.IsNullOrEmpty(content2))
                {
                    after = "値がありません";
                }
                else
                {
                    after = content2;
                }
                changes.Add($"\t\t\t{key},変更前:{prev},変更後:{after}");
                // logger?.LogInformation($"{baseKey}{key}\tItem1:{content1}\tItem2:{content2}");
                result = false;
            }
        }
        if (changes.Any())
        {
            var builder = new StringBuilder();
            builder.AppendLine($"基準となる列の値:{baseKey}");
            builder.AppendLine(string.Join("\n", changes));
            logger?.LogInformation(builder.ToString());
        }
        return result;
    }
}
