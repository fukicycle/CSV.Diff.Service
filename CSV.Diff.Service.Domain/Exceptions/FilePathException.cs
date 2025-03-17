using System;

namespace CSV.Diff.Service.Domain.Exceptions;

public sealed class FilePathException : Exception
{
    public FilePathException(string message) : base(message) { }
}
