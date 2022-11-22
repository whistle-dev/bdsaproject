namespace GitInsight.Infrastructure.Tests;

public class RepoRepositoryTests : IAsyncDisposable
{
    private readonly GitInsightContext _context;
    private readonly SqliteConnection _connection;
    private readonly RepoRepository _repoRepository;

    public RepoRepositoryTests()
    {
        // Setup the in-memory database.
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // Create the options for DbContext instance.
        var builder = new DbContextOptionsBuilder<GitInsightContext>()
            .UseSqlite(_connection);

        // Create the instance of DbContext.
        var context = new GitInsightContext(builder.Options);
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
    public async Task Create_Should_Create_Repo()
    {
        // Arrange
        RepoCreateDTO repo = new RepoCreateDTO("path4", null);

        // Act
        await _repoRepository.CreateAsync(repo);

        // Assert
        _context.Repos.Count().Should().Be(4);
    }

    [Fact]
    public async Task Create_Should_Throw_ArgumentException()
    {
        // Arrange
        RepoCreateDTO repo = new RepoCreateDTO("path1", null);

        // Act
        Func<Task> action = async () => await _repoRepository.CreateAsync(repo);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
        _context.Repos.Count().Should().Be(3);
    }

    [Fact]
    public async Task Find_Should_Return_Repo()
    {
        // Arrange
        string path = "path1";

        // Act
        RepoDTO? repo = await _repoRepository.FindAsync(path);

        // Assert
        repo.Should().NotBeNull();
        repo.Should().BeEquivalentTo(new RepoDTO("path1", null));
    }

    [Fact]
    public async Task Find_Should_Return_Null()
    {
        // Arrange
        string path = "path4";

        // Act
        RepoDTO? repo = await _repoRepository.FindAsync(path);

        // Assert
        repo.Should().BeNull();
    }

    [Fact]
    public async Task Update_Should_Update_Repo()
    {
        // Arrange
        RepoUpdateDTO repo = new RepoUpdateDTO("path1", "sha1");

        // Act
        await _repoRepository.UpdateAsync(repo);

        // Assert
        var r = await _context.Repos.FirstAsync(r => r.Path == "path1");
        r.LatestCommitSha.Should().Be("sha1");
    }

    [Fact]
    public async Task Update_Should_Throw_ArgumentException()
    {
        // Arrange
        RepoUpdateDTO repo = new RepoUpdateDTO("path4", "sha1");

        // Act
        Func<Task> action = async () => await _repoRepository.UpdateAsync(repo);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ReadAll_Should_Return_All_Repos()
    {
        // Arrange
        IReadOnlyCollection<RepoDTO> repos;

        // Act
        repos = await _repoRepository.ReadAllAsync();

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
    public async Task ReadAll_Should_Return_Empty_List()
    {
        // Arrange
        IReadOnlyCollection<RepoDTO> repos;

        // Act
        _context.Repos.RemoveRange(_context.Repos);
        await _context.SaveChangesAsync();
        repos = await _repoRepository.ReadAllAsync();

        // Assert
        repos.Should().NotBeNull();
        repos.Should().BeEmpty();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}