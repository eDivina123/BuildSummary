# Build Summary

![CI](https://github.com/eDivina123/BuildSummary/actions/workflows/ci.yml/badge.svg)

A small command-line tool that summarises CI build results: failure rate,
failures per repo, average build time per branch, and the slowest builds.

## Run it

    dotnet run --project src/BuildSummary.Cli -- samples/builds.json

Or with Docker:

    docker build -t build-summary .
    docker run --rm -v "$(pwd)/samples:/data" build-summary /data/builds.json

## How it's built

- BuildSummary.Core: the summary logic, kept apart from the console so it's easy to test
- BuildSummary.Cli: reads the JSON file and prints the summary
- BuildSummary.Tests: xUnit tests, run by GitHub Actions on every push and pull request

## What I'd add next

- Pull results from a CI system's API instead of a file
- Flag flaky tests that fail and then pass on the same commit
- Output Markdown so the summary can be posted on a pull request
