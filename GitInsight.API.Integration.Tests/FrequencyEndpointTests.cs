namespace GitInsight.API.Integration.Tests;

[Collection("IntegrationTests")]    // Neccessary to run integration tests sequentially, as they share the same tables
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
            { new DateTime(2022, 10, 12).ToShortDateString(), 4 },
            { new DateTime(2022, 10, 18).ToShortDateString(), 5 },
            { new DateTime(2022, 10, 26).ToShortDateString(), 8 },
            { new DateTime(2022, 10, 31).ToShortDateString(), 1 }
        };

        // Act
        var frequencyMode = await _client.GetFromJsonAsync<Dictionary<string, int>>("Frequency" + _testRepo);

        // Assert
        frequencyMode.Should().BeEquivalentTo(expected);
    }
}