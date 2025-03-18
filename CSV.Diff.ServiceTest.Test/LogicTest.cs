using CSV.Diff.Service.Domain.Interfaces;
using CSV.Diff.Service.Domain.Logics;
using Moq;
namespace CSV.Diff.ServiceTest.Test;

public class LogicTest
{
    private Mock<IAppLogger> _loggerMock;
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<IAppLogger>();
    }
    [Test]
    public async Task DiffServiceTest()
    {
        var diffServiceV1 = new DiffService(_loggerMock.Object);
        var diffServiceV2 = new DiffServiceV2(_loggerMock.Object);
        var targetColumns = new string[] { "KEY", "TEST_A", "TEST_B", "TEST_C" };
        var key = "KEY";
        var prevData = new List<IDictionary<string, string?>>();
        var afterData = new List<IDictionary<string, string?>>();

        var prevOneDict = new Dictionary<string, string?>
        {
            { "KEY", "ABC1" },
            { "TEST_A", "a1" },
            { "TEST_B", "b1" },
            { "TEST_C", "c1" },
            { "TEST_D", "d1" }
        };
        var prevTwoDict = new Dictionary<string, string?>{
            { "KEY", "ABC2" },
            { "TEST_A", "a2" },
            { "TEST_B", "b2" },
            { "TEST_C", "c2" },
            { "TEST_D", "d2" }
        };
        var prevThreeDict = new Dictionary<string, string?>{
            { "KEY", "ABC3" },
            { "TEST_A", "a3" },
            { "TEST_B", "b3" },
            { "TEST_C", "c3" },
            { "TEST_D", "d3" }
        };
        prevData.Add(prevOneDict);
        prevData.Add(prevTwoDict);
        prevData.Add(prevThreeDict);

        var afterOneDict = new Dictionary<string, string?>
        {
            { "KEY", "ABC1" },
            { "TEST_A", "a1" },
            { "TEST_B", "b1 - edit" },
            { "TEST_C", "c1" },
            { "TEST_D", "d1" }
        };
        var afterThreeDict = new Dictionary<string, string?>{
            { "KEY", "ABC3" },
            { "TEST_A", "a3" },
            { "TEST_B", "b3" },
            { "TEST_C", "c3" },
            { "TEST_D", "d3 - edit but no tracking." }
        };
        var afterFourDict = new Dictionary<string, string?>{
            { "KEY", "ABC4" },
            { "TEST_A", "a4" },
            { "TEST_B", "b4" },
            { "TEST_C", "c4" },
            { "TEST_D", "d4" }
        };
        afterData.Add(afterOneDict);
        afterData.Add(afterThreeDict);
        afterData.Add(afterFourDict);
        
        var result1 = await diffServiceV1.RunAsync(prevData, afterData, key, targetColumns);
        result1.Deleted.Values.Count.Is(1);
        result1.Added.Values.Count.Is(1);
        result1.Updated.Values.Count.Is(1);

        var result2 = await diffServiceV2.RunAsync(prevData, afterData, key, targetColumns);
        result2.Deleted.Values.Count.Is(1);
        result2.Added.Values.Count.Is(1);
        result2.Updated.Values.Count.Is(1);
    }
}
