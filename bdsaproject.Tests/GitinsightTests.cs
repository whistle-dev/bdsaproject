namespace bdsaproject.Tests;

public class GitInsightTests
{
    public Mock<IRepository> _Repository;

    public GitInsightTests()
    {
        // Initialize mock repository
        _Repository = new MockRepository(MockBehavior.Default).Create<IRepository>();

        // Setup DateTimeOffsets for authors
        DateTimeOffset dateTimeOffset1 = new DateTimeOffset(2022, 10, 28, 0, 0, 0, new TimeSpan(1, 0, 0));
        DateTimeOffset dateTimeOffset2 = new DateTimeOffset(2022, 11, 1, 0, 0, 0, new TimeSpan(1, 0, 0));

        // Setup authors
        Signature committer1 = new Signature("niller", "niller@hotmail.dk", dateTimeOffset1);
        Signature committer2 = new Signature("niller", "niller@hotmail.dk", dateTimeOffset2);
        Signature committer3 = new Signature("lauge-dev", "laupbusiness@gmail.com", dateTimeOffset1);
        Signature committer4 = new Signature("lauge-dev", "laupbusiness@gmail.com", dateTimeOffset2);

        // Setup list of commits
        var commitlist = new List<Commit>();

        // Commit different amount of times for each comitter

        // 2x
        for (int i = 0; i < 2; i++)
        {
            commitlist.Add(setupMockCommit(new Mock<Commit>(), committer1, "committer1 message " + i));
        }

        // 3x
        for (int i = 0; i < 3; i++)
        {
            commitlist.Add(setupMockCommit(new Mock<Commit>(), committer2, "committer2 message " + i));
        }

        // 4x
        for (int i = 0; i < 4; i++)
        {
            commitlist.Add(setupMockCommit(new Mock<Commit>(), committer3, "committer3 message " + i));
        }

        // 5x
        for (int i = 0; i < 5; i++)
        {
            commitlist.Add(setupMockCommit(new Mock<Commit>(), committer4, "committer4 message " + i));
        }

        // Setup mock commits
        var commits = new Mock<IQueryableCommitLog>();

        commits.Setup(c => c.GetEnumerator()).Returns(commitlist.GetEnumerator());

        // Setup mock repository
        _Repository.Setup(r => r.Commits).Returns(commits.Object);
    }

    public Commit setupMockCommit(Mock<Commit> commit, Signature committer, string msg)
    {
        commit.SetupGet(c => c.Committer).Returns(committer);
        commit.SetupGet(c => c.Author).Returns(committer);
        commit.SetupGet(c => c.Message).Returns(msg);
        return commit.Object;
    }

    [Fact]
    public void TestFrequencyMode()
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
    public void TestAuthorMode()
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