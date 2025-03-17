using System.Data;
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
}
