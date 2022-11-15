namespace bdsaproject.Tests;

public class GitInsightTests
{
    public List<CommitDTO> Commits;

    public GitInsightTests()
    {
        Commits = new List<CommitDTO>();

        // Setup Dates
        DateTime date1 = new DateTime(2022, 10, 28);
        DateTime date2 = new DateTime(2022, 11, 1);

        // Setup fake primary keys
        var commitShaIncrement = 0;
        var repoPath = "path";

        // Commit different amount of times for each comitter on each date

        // 2x for niller on date 2022-10-28
        for (int i = 0; i < 2; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Niller date 1 message " + i, date1, "niller", repoPath));
            commitShaIncrement++;
        }

        // 3x for niller on date 2022-11-01
        for (int i = 0; i < 3; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Niller date 2 message " + i, date2, "niller", repoPath));
            commitShaIncrement++;
        }

        // 4x for lauge-dev on date 2022-10-28
        for (int i = 0; i < 4; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Lauge date 1 message " + i, date1, "lauge-dev", repoPath));
            commitShaIncrement++;
        }

        // 5x for lauge-dev on date 2022-11-01
        for (int i = 0; i < 5; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Lauge date 2 message " + i, date2, "lauge-dev", repoPath));
            commitShaIncrement++;
        }
    }

    [Fact]
    public void GitInsight_Constructor_Should_Throw_No_Exceptions()
    {
        // Act
        Action act = () => new app.GitInsight(Commits, 'f');

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void GitInsight_Constructor_Should_Throw_Exception()
    {
        // Act
        Action act = () => new app.GitInsight("wrong/path", 'f');

        // Assert
        act.Should().Throw<Exception>();
        // In client it throws a RepositoryNotFoundException,
        // while on github Actions it throws a System.TypeInitializationException.
    }

    [Fact]
    public void GitInsight_Constructor_Should_Throw_ArgumentException()
    {
        // Act
        Action act = () => new app.GitInsight(Commits, 'x');

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetCommits_Frequency_Mode_Should_Return_Expected_Value()
    {
        // Arrange
        app.GitInsight git = new app.GitInsight(Commits, 'f');

        var expected = new Dictionary<DateTime, int> {
            { new DateTime(2022, 10, 28), 6 },
            { new DateTime(2022, 11, 1), 8 }
        };

        // Act
        var actual = git.getCommits();

        // Assert
        (actual is Dictionary<DateTime, int>).Should().BeTrue();

        var actualDict = (Dictionary<DateTime, int>)actual;
        actualDict.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetCommits_Author_Mode_Should_Return_Expected_Value()
    {
        // Arrange
        app.GitInsight git = new app.GitInsight(Commits, 'a');

        var expected = new Dictionary<string, Dictionary<DateTime, int>> {
            { "niller", new Dictionary<DateTime, int> {
                { new DateTime(2022, 10, 28), 2 },
                { new DateTime(2022, 11, 1), 3 }
            } },
            { "lauge-dev", new Dictionary<DateTime, int> {
                { new DateTime(2022, 10, 28), 4 },
                { new DateTime(2022, 11, 1), 5 }
            } }
        };

        // Act
        var actual = git.getCommits();

        // Assert
        (actual is Dictionary<string, Dictionary<DateTime, int>>).Should().BeTrue();

        var actualDict = (Dictionary<string, Dictionary<DateTime, int>>)actual;
        actualDict.Should().BeEquivalentTo(expected);
    }
}