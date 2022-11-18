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
        Repo repo1 = new Repo("path1");
        Repo repo2 = new Repo("path2");
        Repo repo3 = new Repo("path3");

        context.Repos.AddRange(repo1, repo2, repo3);

        context.SaveChanges();

        _context = context;
        _repoRepository = new RepoRepository(_context);
    }

    [Fact]
    public void Create_Should_Create_Repo()
    {
        // Arrange
        RepoCreateDTO repo = new RepoCreateDTO("path4", null);

        // Act
        _repoRepository.Create(repo);

        // Assert
        _context.Repos.Count().Should().Be(4);
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException()
    {
        // Arrange
        RepoCreateDTO repo = new RepoCreateDTO("path1", null);

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
        string path = "path1";

        // Act
        RepoDTO? repo = _repoRepository.Find(path);

        // Assert
        repo.Should().NotBeNull();
        repo.Should().BeEquivalentTo(new RepoDTO("path1", null));
    }

    [Fact]
    public void Find_Should_Return_Null()
    {
        // Arrange
        string path = "path4";

        // Act
        RepoDTO? repo = _repoRepository.Find(path);

        // Assert
        repo.Should().BeNull();
    }

    [Fact]
    public void Update_Should_Update_Repo()
    {
        // Arrange
        RepoUpdateDTO repo = new RepoUpdateDTO("path1", "sha1");

        // Act
        _repoRepository.Update(repo);

        // Assert
        _context.Repos.First(r => r.Path == "path1").LatestCommitSha.Should().Be("sha1");
    }

    [Fact]
    public void Update_Should_Throw_ArgumentException()
    {
        // Arrange
        RepoUpdateDTO repo = new RepoUpdateDTO("path4", "sha1");

        // Act
        Action action = () => _repoRepository.Update(repo);

        // Assert
        action.Should().Throw<ArgumentException>();
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
            new RepoDTO("path1", null),
            new RepoDTO("path2", null),
            new RepoDTO("path3", null)
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