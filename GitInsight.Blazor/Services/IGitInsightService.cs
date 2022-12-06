namespace GitInsight.Blazor.Services;

public interface IGitInsightService
{
    Task<Dictionary<string, int>> GetCommitsFrequencyAsync(string username, string reponame);
    Task<Dictionary<string, Dictionary<string, int>>> GetCommitsAuthorAsync(string username, string reponame);
    Task<Dictionary<string, Dictionary<string, int>>> GetCommitsFromAuthorAsync(string username, string reponame, string author);
}