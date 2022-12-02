// namespace GitInsight.Blazor.Services;

// public class GitInsightService : IGitInsightService
// {
//     private readonly HttpClient _httpClient;

//     public GitInsightService(HttpClient httpClient)
//     {
//         _httpClient = httpClient;
//     }

//     public async Task<Dictionary<string, int>> GetCommitsFrequencyAsync(string username, string reponame)
//     {
//         var response = await _httpClient.GetAsync($"Frequency/{username}/{reponame}");
//         if (response.IsSuccessStatusCode)
//         {
//             return await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
//         }

//         return null;
//     }

//     public async Task<Dictionary<string, Dictionary<string, int>>> GetCommitsAuthorAsync(string username, string reponame)
//     {
//         var response = await _httpClient.GetAsync($"Author/{username}/{reponame}");
//         if (response.IsSuccessStatusCode)
//         {
//             return await response.Content.ReadFromJsonAsync<Dictionary<string, Dictionary<string, int>>>();
//         }

//         return null;
//     }

//     public async Task<Dictionary<string, int>> GetCommitsFromAuthorAsync(string username, string reponame, string author)
//     {
//         var response = await _httpClient.GetAsync($"Author/{username}/{reponame}/{author}");
//         if (response.IsSuccessStatusCode)
//         {
//             return await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
//         }

//         return null;
//     }



// }