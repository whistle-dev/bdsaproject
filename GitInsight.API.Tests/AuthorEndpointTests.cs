namespace GitInsight.API.Tests;

public class AuthorEndpointTests
{
    private readonly HttpClient _client;

    private readonly string _testRepo = "/whistle-dev/go-ass-3";
    private readonly string _testAuthor = "/2rius";

    public AuthorEndpointTests()
    {
        var factory = new GitInsightWebApplicationFactory();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Commits_Author_Mode()
    {
        // Arrange
        var expected = new Dictionary<string, Dictionary<string, int>> {
            { "12-10-2022", new Dictionary<string, int> {
                { "2rius", 1 },
                { "Rasmus Nielsen", 3 }
            } },
            { "18-10-2022", new Dictionary<string, int> {
                { "2rius", 3 },
                { "Max", 2 }
            } },
            { "26-10-2022", new Dictionary<string, int> {
                { "2rius", 6 },
                { "Rasmus Nielsen", 2 }
            } },
            { "31-10-2022", new Dictionary<string, int> {
                { "mfoman", 1 }
            } }
        };

        // Act
        var authorMode = await _client.GetFromJsonAsync<Dictionary<string, Dictionary<string, int>>>("Author" + _testRepo);

        // Assert
        authorMode.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_Commits_Author_Mode_Specific_Author()
    {
        // Arrange
        var expected = new Dictionary<string, int> {
            { "12-10-2022", 1 },
            { "18-10-2022", 3 },
            { "26-10-2022", 6 }
        };

        // Act
        var authorMode = await _client.GetFromJsonAsync<Dictionary<string, int>>("Author" + _testRepo + _testAuthor);

        // Assert
        authorMode.Should().BeEquivalentTo(expected);
    }
}