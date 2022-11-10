namespace GitInsight.Infrastructure.Tests;

public class AuthorRepositoryTests : IDisposable
{
    private readonly GitinsightContext _context;
    private readonly AuthorRepository _authorRepository;

    public AuthorRepositoryTests()
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
        Author author1 = new Author("hash1", "name1", "email1");
        Author author2 = new Author("hash2", "name2", "email2");
        Author author3 = new Author("hash3", "name3", "email3");

        context.Authors.AddRange(author1, author2, author3);

        context.SaveChanges();

        _context = context;
        _authorRepository = new AuthorRepository(_context);
    }

    [Fact]
    public void Create_Should_Create_Author()
    {
        // Arrange
        AuthorCreateDTO author = new AuthorCreateDTO("hash4", "name4", "email4");

        // Act
        _authorRepository.Create(author);

        // Assert
        _context.Authors.Count().Should().Be(4);
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException()
    {
        // Arrange
        AuthorCreateDTO author = new AuthorCreateDTO("hash1", "name1", "email1");

        // Act
        Action action = () => _authorRepository.Create(author);

        // Assert
        action.Should().Throw<ArgumentException>();
        _context.Authors.Count().Should().Be(3);
    }

    [Fact]
    public void Find_Should_Return_Author()
    {
        // Arrange
        string hash = "hash1";

        // Act
        AuthorDTO? author = _authorRepository.Find(hash);

        // Assert
        author.Should().NotBeNull();
        author.Should().BeEquivalentTo(new AuthorDTO("hash1", "name1", "email1"));
    }

    [Fact]
    public void Find_Should_Return_Null()
    {
        // Arrange
        string hash = "hash4";

        // Act
        AuthorDTO? author = _authorRepository.Find(hash);

        // Assert
        author.Should().BeNull();
    }

    [Fact]
    public void ReadAll_Should_Return_All_Authors()
    {
        // Arrange
        IReadOnlyCollection<AuthorDTO> authors;

        // Act
        authors = _authorRepository.ReadAll();

        // Assert
        authors.Should().NotBeNull();
        authors.Should().HaveCount(3);
        authors.Should().BeEquivalentTo(new List<AuthorDTO>()
        {
            new AuthorDTO("hash1", "name1", "email1"),
            new AuthorDTO("hash2", "name2", "email2"),
            new AuthorDTO("hash3", "name3", "email3")
        });
    }

    [Fact]
    public void ReadAll_Should_Return_Empty_List()
    {
        // Arrange
        IReadOnlyCollection<AuthorDTO> authors;

        // Act
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();
        authors = _authorRepository.ReadAll();

        // Assert
        authors.Should().NotBeNull();
        authors.Should().BeEmpty();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}