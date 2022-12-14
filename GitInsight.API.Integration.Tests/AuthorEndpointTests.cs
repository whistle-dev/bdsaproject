namespace GitInsight.API.Integration.Tests;

[Collection("IntegrationTests")]    // Neccessary to run integration tests sequentially, as they share the same tables
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
            { "2rius", new Dictionary<string, int> {
                { new DateTime(2022, 10, 12).ToShortDateString(), 1 },
                { new DateTime(2022, 10, 18).ToShortDateString(), 3 },
                { new DateTime(2022, 10, 26).ToShortDateString(), 6 }
            } },
            { "Rasmus Nielsen", new Dictionary<string, int> {
                { new DateTime(2022, 10, 12).ToShortDateString(), 3 },
                { new DateTime(2022, 10, 26).ToShortDateString(), 2 }
            } },
            { "Max", new Dictionary<string, int> {
                { new DateTime(2022, 10, 18).ToShortDateString(), 2 }
            } },
            { "mfoman", new Dictionary<string, int> {
                { new DateTime(2022, 10, 31).ToShortDateString(), 1 }
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
            { new DateTime(2022, 10, 12).ToShortDateString(), 1 },
            { new DateTime(2022, 10, 18).ToShortDateString(), 3 },
            { new DateTime(2022, 10, 26).ToShortDateString(), 6 }
        };

        // Act
        var authorMode = await _client.GetFromJsonAsync<Dictionary<string, int>>("Author" + _testRepo + _testAuthor);

        // Assert
        authorMode.Should().BeEquivalentTo(expected);
    }
}