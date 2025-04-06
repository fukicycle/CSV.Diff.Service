using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.Logics;
using Moq;
namespace CSV.Diff.ServiceTest.Test;

public class LogicTest
{
    private Mock<IAppLogger> _mockLogger = null!;
    private DiffServiceV2 _diffService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<IAppLogger>();
        _diffService = new DiffServiceV2(_mockLogger.Object);
    }

    [Test]
    public async Task RunAsync_ShouldIdentifyAddedDeletedAndUpdatedData()
    {
        // Arrange
        var prevData = new List<IDictionary<string, string?>>
            {
                new Dictionary<string, string?> { { "id", "1" }, { "name", "Alice" }, { "age", "30" } },
                new Dictionary<string, string?> { { "id", "2" }, { "name", "Bob" }, { "age", "40" } }
            };

        var afterData = new List<IDictionary<string, string?>>
            {
                new Dictionary<string, string?> { { "id", "2" }, { "name", "Bob" }, { "age", "41" } }, // Updated
                new Dictionary<string, string?> { { "id", "3" }, { "name", "Charlie" }, { "age", "25" } } // Added
            };

        var targetColumns = new[] { "id", "name", "age" };

        // Act
        var result = await _diffService.RunAsync(prevData, afterData, "id", targetColumns);

        // Assert with Chaining-Assertion
        result.Added.Count().Is(1);
        result.Added.First().BaseValue.Is("3");

        result.Deleted.Count().Is(1);
        result.Deleted.First().BaseValue.Is("1");

        result.Updated.Count().Is(1);
        result.Updated.First().BaseValue.Is("2");
        result.Updated.First().Changes.Count().Is(1);

        var change = result.Updated.First().Changes.First();
        change.Column.Is("age");
        change.Prev.Is("40");
        change.After.Is("41");

        // Logger の検証
        _mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        _mockLogger.Verify(x => x.LogDebug(It.Is<string>(s => s.Contains("Change!2,40,41"))), Times.Once);
    }

    [Test]
    public void RunAsync_ShouldThrowException_WhenBaseColumnValueIsNull()
    {
        // Arrange
        var prevData = new List<IDictionary<string, string?>>
            {
                new Dictionary<string, string?> { { "id", null }, { "name", "Alice" } }
            };

        var afterData = new List<IDictionary<string, string?>>
            {
                new Dictionary<string, string?> { { "id", "1" }, { "name", "Alice" } }
            };

        var targetColumns = new[] { "id", "name" };

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(() =>
            _diffService.RunAsync(prevData, afterData, "id", targetColumns));

        ex!.Message.Is("基準となる列の値が入っていません。");
    }
}
