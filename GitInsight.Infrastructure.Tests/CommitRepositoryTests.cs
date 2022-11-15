namespace GitInsight.Infrastructure.Tests;

public class CommitRepositoryTests : IDisposable
{
    private readonly GitinsightContext _context;
    private readonly CommitRepository _commitRepository;

    public CommitRepositoryTests()
    {
        // Setup the in-memory database.
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        // Create the options for DbContext instance.
        var builder = new DbContextOptionsBuilder<GitinsightContext>()
            .UseSqlite(connection);

        // Create the instance of DbContext.
        var context = new GitinsightContext(builder.Options);
        context.Database.EnsureCreated();

        // Seed the database with test data.

        Repo repo1 = new Repo(1, "name1");
        Repo repo2 = new Repo(2, "name2");
        Repo repo3 = new Repo(3, "name3");

        context.Repos.AddRange(repo1, repo2, repo3);

        Commit commit1 = new Commit(1, "message1", new DateTime(2022, 10, 28), "name1") { RepoHash = 1, Repo = repo1 };
        Commit commit2 = new Commit(2, "message2", new DateTime(2022, 10, 28), "name2") { RepoHash = 1, Repo = repo1 };
        Commit commit3 = new Commit(3, "message3", new DateTime(2022, 10, 28), "name2") { RepoHash = 1, Repo = repo1 };
        Commit commit4 = new Commit(4, "message4", new DateTime(2022, 10, 28), "name3") { RepoHash = 1, Repo = repo1 };
        Commit commit5 = new Commit(5, "message5", new DateTime(2022, 10, 28), "name3") { RepoHash = 2, Repo = repo2 };
        Commit commit6 = new Commit(6, "message6", new DateTime(2022, 10, 28), "name4") { RepoHash = 2, Repo = repo2 };
        Commit commit7 = new Commit(7, "message7", new DateTime(2022, 10, 28), "name4") { RepoHash = 2, Repo = repo2 };
        Commit commit8 = new Commit(8, "message8", new DateTime(2022, 11, 1), "name2") { RepoHash = 1, Repo = repo1 };
        Commit commit9 = new Commit(9, "message9", new DateTime(2022, 11, 2), "name3") { RepoHash = 2, Repo = repo2 };

        context.Commits.AddRange(commit1, commit2, commit3, commit4, commit5, commit6, commit7, commit8, commit9);

        context.SaveChanges();

        _context = context;
        _commitRepository = new CommitRepository(_context);
    }

    [Fact]
    public void Create_Should_Create_Commit()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO(10, "message10", new DateTime(2022, 11, 1), "name1", 1);

        // Act
        _commitRepository.Create(commit);

        // Assert
        _context.Commits.Count().Should().Be(10);
    }

    [Fact]
    public void Create_Should_Not_Create_Duplicate_Commit()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO(1, "message1", new DateTime(2022, 10, 28), "name1", 1);

        // Act
        _commitRepository.Create(commit);

        // Assert
        _context.Commits.Count().Should().Be(9);
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException_With_Missing_Repo_Message()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO(10, "message10", new DateTime(2022, 11, 1), "name1", 10);

        // Act
        Action action = () => _commitRepository.Create(commit);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Repo does not exist in database.");
    }

    [Fact]
    public void Find_Should_Return_Commit()
    {
        // Arrange
        int hash = 1;

        // Act
        CommitDTO? commit = _commitRepository.Find(hash);

        // Assert
        commit.Should().NotBeNull();
        commit.Should().BeEquivalentTo(new CommitDTO(1, "message1", new DateTime(2022, 10, 28), "name1", 1));
    }

    [Fact]
    public void Find_Should_Return_Null()
    {
        // Arrange
        int hash = 10;

        // Act
        CommitDTO? commit = _commitRepository.Find(hash);

        // Assert
        commit.Should().BeNull();
    }

    [Fact]
    public void ReadAllInRepo_Should_Return_All_Commits_In_Repo()
    {
        // Arrange
        int repo = 1;

        // Act
        IReadOnlyCollection<CommitDTO> commits = _commitRepository.ReadAllInRepo(repo);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(5);
        commits.Should().BeEquivalentTo(new List<CommitDTO>()
        {
            new CommitDTO(1, "message1", new DateTime(2022, 10, 28), "name1", 1),
            new CommitDTO(2, "message2", new DateTime(2022, 10, 28), "name2", 1),
            new CommitDTO(3, "message3", new DateTime(2022, 10, 28), "name2", 1),
            new CommitDTO(4, "message4", new DateTime(2022, 10, 28), "name3", 1),
            new CommitDTO(8, "message8", new DateTime(2022, 11, 1), "name2", 1),
        });
    }

    [Fact]
    public void ReadAllInRepo_Should_Return_Empty_List()
    {
        // Arrange
        int repo = 3;

        // Act
        IReadOnlyCollection<CommitDTO> commits = _commitRepository.ReadAllInRepo(repo);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(0);
    }

    [Fact]
    public void ReadAllInRepo_Should_Throw_ArgumentException()
    {
        // Arrange
        int repo = 4;

        // Act
        Action action = () => _commitRepository.ReadAllInRepo(repo);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}