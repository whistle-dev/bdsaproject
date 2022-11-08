namespace bdsaproject.Tests;

public class GitInsightTests
{
    public Mock<IRepository> _Repository;

    public GitInsightTests()
    {
        // Initialize mock repository
        _Repository = new MockRepository(MockBehavior.Default).Create<IRepository>();

        // Setup DateTimeOffsets for authors
        DateTimeOffset date1 = new DateTimeOffset(2022, 10, 28, 0, 0, 0, new TimeSpan(1, 0, 0));
        DateTimeOffset date2 = new DateTimeOffset(2022, 11, 1, 0, 0, 0, new TimeSpan(1, 0, 0));

        // Setup authors
        Signature nillerDate1 = new Signature("niller", "niller@hotmail.dk", date1);
        Signature nillerDate2 = new Signature("niller", "niller@hotmail.dk", date2);
        Signature laugeDate1 = new Signature("lauge-dev", "laupbusiness@gmail.com", date1);
        Signature laugeDate2 = new Signature("lauge-dev", "laupbusiness@gmail.com", date2);

        // Setup list of commits
        var commitlist = new List<Commit>();

        // Commit different amount of times for each comitter

        // 2x for niller on date 2022-10-28
        for (int i = 0; i < 2; i++) { commitlist.Add(SetupMockCommit(new Mock<Commit>(), nillerDate1, "Niller date 1 message " + i)); }

        // 3x for niller on date 2022-11-01
        for (int i = 0; i < 3; i++) { commitlist.Add(SetupMockCommit(new Mock<Commit>(), nillerDate2, "Niller date 2 message " + i)); }

        // 4x for lauge-dev on date 2022-10-28
        for (int i = 0; i < 4; i++) { commitlist.Add(SetupMockCommit(new Mock<Commit>(), laugeDate1, "Lauge date 1 message " + i)); }

        // 5x for lauge-dev on date 2022-11-01
        for (int i = 0; i < 5; i++) { commitlist.Add(SetupMockCommit(new Mock<Commit>(), laugeDate2, "Lauge date 2 message " + i)); }

        // Setup mock commits
        var commits = new Mock<IQueryableCommitLog>();

        commits.Setup(c => c.GetEnumerator()).Returns(commitlist.GetEnumerator());

        // Setup mock repository
        _Repository.Setup(r => r.Commits).Returns(commits.Object);
    }

    // Method to avoid redundant code for setup of mock commits
    public Commit SetupMockCommit(Mock<Commit> commit, Signature committer, string msg)
    {
        commit.SetupGet(c => c.Committer).Returns(committer);
        commit.SetupGet(c => c.Author).Returns(committer);
        commit.SetupGet(c => c.Message).Returns(msg);
        return commit.Object;
    }

    [Fact]
    public void GitInsight_Constructor_Throws_No_Exceptions()
    {
        // Act
        Action act = () => new GitInsight(_Repository.Object, 'f');

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void GitInsight_Constructor_Throws_RepositoryNotFoundException()
    {
        // Act
        Action act = () => new GitInsight("wrong/path", 'f');

        // Assert
        act.Should().Throw<RepositoryNotFoundException>();
    }

    [Fact]
    public void GitInsight_Constructor_Throws_ArgumentException()
    {
        // Act
        Action act = () => new GitInsight(_Repository.Object, 'x');

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetCommits_Frequency_Mode_Returns_Expected_Value()
    {
        // Arrange
        GitInsight git = new GitInsight(_Repository.Object, 'f');

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
    public void GetCommits_Author_Mode_Returns_Expected_Value()
    {
        // Arrange
        GitInsight git = new GitInsight(_Repository.Object, 'a');

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