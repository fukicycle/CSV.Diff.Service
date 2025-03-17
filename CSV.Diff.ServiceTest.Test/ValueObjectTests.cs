using CSV.Diff.Service.Domain.Exceptions;
using CSV.Diff.Service.Domain.ValueObjects;

namespace CSV.Diff.ServiceTest.Test;

public class ValueObjectTests
{
    [Test]
    public void FilePath()
    {
        //error case
        var ex = Assert.Throws<FilePathException>(() => new FilePath(""));
        ex.Message.Is("Specified file is not found. ()");

        var ex1 = Assert.Throws<FilePathException>(() => new FilePath("TEST.csv"));
        ex1.Message.Is("Specified file is not found. (TEST.csv)");

        //success case
        File.Create("TEST.txt").Dispose();
        var filePath = new FilePath("TEST.txt");
        filePath.Value.Is(Path.Combine(Environment.CurrentDirectory, "TEST.txt"));
        filePath.ShortName.Is("TEST.txt");
        filePath.Value.GetHashCode().Is(Path.Combine(Environment.CurrentDirectory, "TEST.txt").GetHashCode());
        filePath.Equals(filePath).Is(true);
        File.Delete("TEST.txt");
    }
}