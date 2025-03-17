using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.ValueObjects;

namespace CSV.Diff.Service.Domain.Interfaces;

public interface IResultWriter
{
    Task WriteAsync(string targetFileName, DiffResultContent content);
}
