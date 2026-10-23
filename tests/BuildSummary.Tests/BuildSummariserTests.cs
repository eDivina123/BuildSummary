using BuildSummary.Core;
namespace BuildSummary.Tests;

//arrange, act and then assert
public class BuildSummariserTests
{
    private static readonly List<Build> Builds = new()
    {
        new("api", "main", "Failed", 10),
        new("api", "main", "Succeeded", 8),
        new("web", "dev", "Failed", 5),
        new("api", "dev", "Failed", 12),
    };

    [Fact]
    public void FailuresByRepo_PutsWorstRepoFirst()
    {
        var result = BuildSummariser.FailuresByRepo(Builds);

        Assert.Equal("api", result[0].Repo);
        Assert.Equal(2, result[0].Failures);
    }

    [Fact]
    public void FailureRate_IsZeroForNoBuilds()
    {
        Assert.Equal(0.0, BuildSummariser.FailureRate(new List<Build>()));
    }

    [Fact]
    public void FailureRate_IsCorrectForSomeBuilds()
    {
        Assert.Equal(0.5, BuildSummariser.FailureRate(Builds));
    }

    [Fact]
    public void AverageMinuteByBranch_IsCorrect()
    {
        var result = BuildSummariser.AverageMinutesByBranch(Builds);
        Assert.Equal(9.0, result["main"]);
        Assert.Equal(8.5, result["dev"]);
        
    }

    [Fact]
    public void Slowest_ReturnsCorrectNumberOfBuilds()
    {
        var result = BuildSummariser.Slowest(Builds, 2);
        Assert.Equal(12, result[0].Minutes);
        Assert.Equal(10, result[1].Minutes);
    }

    [Fact]
    public void Slowest_ReturnsAllBuildsWhenCountTooLarge()
    {
        var result = BuildSummariser.Slowest(Builds,5);
        Assert.Equal(4, result.Count);
      
    }
}
