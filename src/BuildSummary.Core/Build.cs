namespace BuildSummary.Core;

public record Build(
    string Repo,
    string Branch,
    string Status,
    double Minutes
);