using System.Text.Json; 
using BuildSummary.Core;

//program exit code is 0 for success, 1 for failure
if( args.Length != 1)
{
    Console.WriteLine("Usage: BuildSummary.Cli <path to builds.json>");
    return 1;
} 


var path = args[0];
if(!File.Exists(path))
{
    Console.WriteLine($"File not found: {path}");
    return 1;
}

var json = File.ReadAllText(path);
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
var builds = JsonSerializer.Deserialize<List<Build>>(json, options);

//summary statistics 
if (builds == null)
{
    Console.WriteLine("No builds found in the file.");
    return 1;
}

int count = builds.Count;
double failureRate = BuildSummariser.FailureRate(builds);
var failuresByRepo = BuildSummariser.FailuresByRepo(builds);
var averageMinutesByBranch = BuildSummariser.AverageMinutesByBranch(builds);
var slowestBuilds = BuildSummariser.Slowest(builds, 3);

Console.WriteLine($"{count} builds, {failureRate:P0} failed");
Console.WriteLine("Failures by repo:");
foreach (var (repo, failures) in failuresByRepo)
{
    Console.WriteLine($"  {repo}: {failures}");
}

Console.WriteLine("Average build time by branch:");
foreach (var (branch, avgMinutes) in averageMinutesByBranch)
{
    Console.WriteLine($"  {branch}: {avgMinutes:F1} minutes");
}

Console.WriteLine("Slowest builds:");
foreach (var build in slowestBuilds)
{
    Console.WriteLine($"  {build.Repo} ({build.Branch}): {build.Minutes:F1} min");
}

return 0;


