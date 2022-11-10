namespace GitInsight.Infrastructure.Tests;

public class RepoRepositoryTests : IDisposable
{
    private readonly GitinsightContext _context;
    private readonly RepoRepository _repoRepository;

    public RepoRepositoryTests()
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
        Repo repo1 = new Repo("hash1","name1");
        Repo repo2 = new Repo("hash2", "name2");
        Repo repo3 = new Repo("hash3", "name3");

        context.Repos.AddRange(repo1, repo2, repo3);

        context.SaveChanges();

        _context = context;
        _repoRepository = new RepoRepository(_context);
    }

    [Fact]
    public void Create_Should_Create_Repo()
    {
        // Arrange
        RepoCreateDTO repo = new RepoCreateDTO("hash4", "name4");

        // Act
        _repoRepository.Create(repo);

        // Assert
        _context.Repos.Count().Should().Be(4);
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException()
    {
        // Arrange
        RepoCreateDTO repo = new RepoCreateDTO("hash1", "name1");

        // Act
        Action action = () => _repoRepository.Create(repo);

        // Assert
        action.Should().Throw<ArgumentException>();
        _context.Repos.Count().Should().Be(3);
    }

    [Fact]
    public void Find_Should_Return_Repo()
    {
        // Arrange
        string hash = "hash1";

        // Act
        RepoDTO? repo = _repoRepository.Find(hash);

        // Assert
        repo.Should().NotBeNull();
        repo.Should().BeEquivalentTo(new RepoDTO("hash1", "name1"));
    }

    [Fact]
    public void Find_Should_Return_Null()
    {
        // Arrange
        string hash = "hash4";

        // Act
        RepoDTO? repo = _repoRepository.Find(hash);

        // Assert
        repo.Should().BeNull();
    }

    [Fact]
    public void ReadAll_Should_Return_All_Repos()
    {
        // Arrange
        IReadOnlyCollection<RepoDTO> repos;

        // Act
        repos = _repoRepository.ReadAll();

        // Assert
        repos.Should().NotBeNull();
        repos.Should().HaveCount(3);
        repos.Should().BeEquivalentTo(new List<RepoDTO>()
        {
            new RepoDTO("hash1", "name1"),
            new RepoDTO("hash2", "name2"),
            new RepoDTO("hash3", "name3")
        });
    }

    [Fact]
    public void ReadAll_Should_Return_Empty_List()
    {
        // Arrange
        IReadOnlyCollection<RepoDTO> repos;

        // Act
        _context.Repos.RemoveRange(_context.Repos);
        _context.SaveChanges();
        repos = _repoRepository.ReadAll();

        // Assert
        repos.Should().NotBeNull();
        repos.Should().BeEmpty();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}