namespace BuildSummary.Core;

public static class BuildSummariser
{
    public static List<(string Repo, int Failures)> FailuresByRepo(IEnumerable<Build> builds)
    {   
        //linq version but can do for loops
        return builds
            .Where(b => b.Status.ToLower() == "failed")
            .GroupBy(b => b.Repo)
            .Select(g => (Repo: g.Key, Failures: g.Count()))
            .OrderByDescending(x => x.Failures)
            .ToList();
    }

    //ireadonlylist but we need count so we use ireadolycollection instead of ienumerable
    public static double FailureRate(IReadOnlyCollection<Build> builds)
    {
        //<Nullable>enable</Nullable> in the .csproj. That makes IReadOnlyCollection<Build> mean "never null,
        if (builds.Count == 0)
        {
            return 0.0;
        }

        double numFailures = builds.Count(b => b.Status.ToLower() == "failed");
        return numFailures / builds.Count;
    }

    public static Dictionary<string, double> AverageMinutesByBranch(IEnumerable<Build> builds)
    {
        return builds
        .GroupBy(b => b.Branch)
        .ToDictionary(
            g => g.Key, 
            g => g.Average(b => b.Minutes)
        );
       
    }

    public static List<Build> Slowest(IEnumerable<Build> builds, int n)
    {
        return builds
            .OrderByDescending(b => b.Minutes)
            .Take(n)
            .ToList();
    }

    
}