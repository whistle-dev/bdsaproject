namespace GitInsight.API.Tests;

public class GitInsightTests
{
    public List<CommitDTO> Commits;
    public string Path;

    public GitInsightTests()
    {
        Commits = new List<CommitDTO>();

        // Setup Dates
        DateTime date1 = new DateTime(2022, 10, 28);
        DateTime date2 = new DateTime(2022, 11, 1);

        // Setup fake primary keys
        var commitShaIncrement = 0;
        Path = "path";

        // Commit different amount of times for each comitter on each date

        // 2x for niller on date 2022-10-28
        for (int i = 0; i < 2; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Niller date 1 message " + i, date1, "niller", Path));
            commitShaIncrement++;
        }

        // 3x for niller on date 2022-11-01
        for (int i = 0; i < 3; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Niller date 2 message " + i, date2, "niller", Path));
            commitShaIncrement++;
        }

        // 4x for lauge-dev on date 2022-10-28
        for (int i = 0; i < 4; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Lauge date 1 message " + i, date1, "lauge-dev", Path));
            commitShaIncrement++;
        }

        // 5x for lauge-dev on date 2022-11-01
        for (int i = 0; i < 5; i++)
        {
            Commits.Add(new CommitDTO("sha" + commitShaIncrement, "Lauge date 2 message " + i, date2, "lauge-dev", Path));
            commitShaIncrement++;
        }
    }

    [Fact]
    public void GitInsight_Constructor_Should_Throw_No_Exceptions()
    {
        // Act
        Action act = () => new GitInsight(Commits, 'f', Path);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void GitInsight_Constructor_Should_Throw_ArgumentException()
    {
        // Act
        Action act = () => new GitInsight(Commits, 'x', Path);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetCommits_Frequency_Mode_Should_Return_Expected_Value()
    {
        // Arrange
        GitInsight git = new GitInsight(Commits, 'f', Path);

        var expected = new Dictionary<string, int> {
            { new DateTime(2022, 10, 28).ToString("dd-MM-yyyy"), 6 },
            { new DateTime(2022, 11, 1).ToString("dd-MM-yyyy"), 8 }
        };

        // Act
        var actual = git.getCommitsFrequency();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetCommits_Author_Mode_Should_Return_Expected_Value()
    {
        // Arrange
        GitInsight git = new GitInsight(Commits, 'a', Path);

        var expected = new Dictionary<string, Dictionary<string, int>> {
            { "niller", new Dictionary<string, int> {
                { new DateTime(2022, 10, 28).ToString("dd-MM-yyyy"), 2 },
                { new DateTime(2022, 11, 1).ToString("dd-MM-yyyy"), 3 }
            } },
            { "lauge-dev", new Dictionary<string, int> {
                { new DateTime(2022, 10, 28).ToString("dd-MM-yyyy"), 4 },
                { new DateTime(2022, 11, 1).ToString("dd-MM-yyyy"), 5 }
            } }
        };

        // Act
        var actual = git.getCommitsAuthor();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetCommitsFromAuthor_Should_Return_Expected_Value()
    {
        // Arrange
        GitInsight git = new GitInsight(Commits, 'a', Path);

        var expected = new Dictionary<string, int> {
            { new DateTime(2022, 10, 28).ToString("dd-MM-yyyy"), 2 },
            { new DateTime(2022, 11, 1).ToString("dd-MM-yyyy"), 3 }
        };

        // Act
        var actual = git.getCommitsFromAuthor("niller");

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}