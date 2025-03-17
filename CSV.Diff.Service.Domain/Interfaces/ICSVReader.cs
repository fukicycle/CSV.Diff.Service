using System;
using CSV.Diff.Service.Domain.Entities;
using CSV.Diff.Service.Domain.ValueObjects;

namespace CSV.Diff.Service.Domain.Interfaces;

public interface ICSVReader
{
    Task<CSVContent> ReadAsync(FilePath filePath);
}
