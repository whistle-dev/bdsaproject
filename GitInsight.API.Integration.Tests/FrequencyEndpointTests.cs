namespace GitInsight.API.Integration.Tests;

public class FrequencyEndpointTests
{
    private readonly HttpClient _client;

    private readonly string _testRepo = "/whistle-dev/go-ass-3";

    public FrequencyEndpointTests()
    {
        var factory = new GitInsightWebApplicationFactory();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Commits_Frequency_Mode()
    {
        // Arrange
        var expected = new Dictionary<string, int> {
            { "12-10-2022", 4 },
            { "18-10-2022", 5 },
            { "26-10-2022", 8 },
            { "31-10-2022", 1 }
        };

        // Act
        var frequencyMode = await _client.GetFromJsonAsync<Dictionary<string, int>>("Frequency" + _testRepo);

        // Assert
        frequencyMode.Should().BeEquivalentTo(expected);
    }
}