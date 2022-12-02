namespace GitInsight.Blazor.Services;
using System.Net.Http;
using System.Net.Http.Json;

public class GitInsightService : IGitInsightService
{
    private readonly HttpClient _httpClient;

    public GitInsightService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<Dictionary<string, int>> GetCommitsFrequencyAsync(string username, string reponame)
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>($"api/frequency/{username}/{reponame}");
    }

    public async Task<Dictionary<string, Dictionary<string, int>>> GetCommitsAuthorAsync(string username, string reponame)
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, int>>>($"api/author/{username}/{reponame}");
    }

    public async Task<Dictionary<string, int>> GetCommitsFromAuthorAsync(string username, string reponame, string author)
    {
        return await _httpClient.GetFromJsonAsync<Dictionary<string, int>>($"api/author/{username}/{reponame}/{author}");
    }
}