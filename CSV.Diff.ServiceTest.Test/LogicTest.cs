using CSV.Diff.Service.Domain.Logics;
namespace CSV.Diff.ServiceTest.Test;

public class LogicTest
{
    [Test]
    public async Task DiffServiceTest()
    {
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

        var result = await DiffService.RunAsync(prevData, afterData, key, targetColumns);
        result.Deleted.Values.Count.Is(1);
        result.Added.Values.Count.Is(1);
        result.Updated.Values.Count.Is(1);
    }
}
