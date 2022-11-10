namespace GitInsight.Infrastructure.Tests;

public class CommitRepositoryTests : IDisposable
{
    private readonly GitinsightContext _context;
    private readonly CommitRepository _CommitRepository;

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
        Author author1 = new Author("hash1", "name1", "email1");
        Author author2 = new Author("hash2", "name2", "email2");
        Author author3 = new Author("hash3", "name3", "email3");
        Author author4 = new Author("hash4", "name4", "email4");

        context.Authors.AddRange(author1, author2, author3);

        Repo repo1 = new Repo("hash1", "name1");
        Repo repo2 = new Repo("hash2", "name2");
        Repo repo3 = new Repo("hash3", "name3");

        context.Repos.AddRange(repo1, repo2, repo3);

        Commit commit1 = new Commit("hash1", "message1", new DateTime(2022, 10, 28)) { AuthorHash = "hash1", RepoHash = "hash1", Author = author1, Repo = repo1 };
        Commit commit2 = new Commit("hash2", "message2", new DateTime(2022, 10, 28)) { AuthorHash = "hash2", RepoHash = "hash1", Author = author2, Repo = repo1 };
        Commit commit3 = new Commit("hash3", "message3", new DateTime(2022, 10, 28)) { AuthorHash = "hash2", RepoHash = "hash1", Author = author2, Repo = repo1 };
        Commit commit4 = new Commit("hash4", "message4", new DateTime(2022, 10, 28)) { AuthorHash = "hash3", RepoHash = "hash1", Author = author3, Repo = repo1 };
        Commit commit5 = new Commit("hash5", "message5", new DateTime(2022, 10, 28)) { AuthorHash = "hash3", RepoHash = "hash2", Author = author3, Repo = repo2 };
        Commit commit6 = new Commit("hash6", "message6", new DateTime(2022, 10, 28)) { AuthorHash = "hash4", RepoHash = "hash2", Author = author4, Repo = repo2 };
        Commit commit7 = new Commit("hash7", "message7", new DateTime(2022, 10, 28)) { AuthorHash = "hash4", RepoHash = "hash2", Author = author4, Repo = repo2 };
        Commit commit8 = new Commit("hash8", "message8", new DateTime(2022, 11, 1)) { AuthorHash = "hash2", RepoHash = "hash1", Author = author2, Repo = repo1 };
        Commit commit9 = new Commit("hash9", "message9", new DateTime(2022, 11, 2)) { AuthorHash = "hash3", RepoHash = "hash2", Author = author3, Repo = repo2 };

        context.Commits.AddRange(commit1, commit2, commit3, commit4, commit5, commit6, commit7, commit8, commit9);

        context.SaveChanges();

        _context = context;
        _CommitRepository = new CommitRepository(_context);
    }

    [Fact]
    public void Create_Should_Create_Commit()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO("hash10", "message10", new DateTime(2022, 11, 1), "hash1", "hash1");

        // Act
        _CommitRepository.Create(commit);

        // Assert
        _context.Commits.Count().Should().Be(10);
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException_With_Already_Exists_Message()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO("hash1", "message1", new DateTime(2022, 10, 28), "hash1", "hash1");

        // Act
        Action action = () => _CommitRepository.Create(commit);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Commit already exists in database.");
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException_With_Missing_Repo_Message()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO("hash10", "message10", new DateTime(2022, 11, 1), "hash1", "hash10");

        // Act
        Action action = () => _CommitRepository.Create(commit);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Repo does not exist in database.");
    }

    [Fact]
    public void Create_Should_Throw_ArgumentException_With_Missing_Author_Message()
    {
        // Arrange
        CommitCreateDTO commit = new CommitCreateDTO("hash10", "message10", new DateTime(2022, 11, 1), "hash10", "hash1");

        // Act
        Action action = () => _CommitRepository.Create(commit);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Author does not exist in database.");
    }

    [Fact]
    public void Find_Should_Return_Commit()
    {
        // Arrange
        string hash = "hash1";

        // Act
        CommitDTO? commit = _CommitRepository.Find(hash);

        // Assert
        commit.Should().NotBeNull();
        commit.Should().BeEquivalentTo(new CommitDTO("hash1", "message1", new DateTime(2022, 10, 28), "hash1", "hash1"));
    }

    [Fact]
    public void Find_Should_Return_Null()
    {
        // Arrange
        string hash = "hash10";

        // Act
        CommitDTO? commit = _CommitRepository.Find(hash);

        // Assert
        commit.Should().BeNull();
    }

    [Fact]
    public void ReadAllInRepo_Should_Return_All_Commits_In_Repo()
    {
        // Arrange
        string repo = "hash1";

        // Act
        IReadOnlyCollection<CommitDTO> commits = _CommitRepository.ReadAllInRepo(repo);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(5);
        commits.Should().BeEquivalentTo(new List<CommitDTO>()
        {
            new CommitDTO("hash1", "message1", new DateTime(2022, 10, 28), "hash1", "hash1"),
            new CommitDTO("hash2", "message2", new DateTime(2022, 10, 28), "hash2", "hash1"),
            new CommitDTO("hash3", "message3", new DateTime(2022, 10, 28), "hash2", "hash1"),
            new CommitDTO("hash4", "message4", new DateTime(2022, 10, 28), "hash3", "hash1"),
            new CommitDTO("hash8", "message8", new DateTime(2022, 11, 1), "hash2", "hash1"),
        });
    }

    [Fact]
    public void ReadAllInRepo_Should_Return_Empty_List()
    {
        // Arrange
        string repo = "hash3";

        // Act
        IReadOnlyCollection<CommitDTO> commits = _CommitRepository.ReadAllInRepo(repo);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(0);
    }

    [Fact]
    public void ReadAllInRepo_Should_Throw_ArgumentException()
    {
        // Arrange
        string repo = "hash4";

        // Act
        Action action = () => _CommitRepository.ReadAllInRepo(repo);

        // Assert
        action.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ReadAllUniqueDatesInRepo_Should_Return_All_Dates_In_Repo()
    {
        // Arrange
        string repo = "hash1";

        // Act
        IReadOnlyCollection<DateTime> dates = _CommitRepository.ReadAllUniqueDatesInRepo(repo);

        // Assert
        dates.Should().NotBeNull();
        dates.Count.Should().Be(2);
        dates.Should().BeEquivalentTo(new List<DateTime>()
        {
            new DateTime(2022, 10, 28),
            new DateTime(2022, 11, 1),
        });
    }

    [Fact]
    public void ReadAllUniqueDatesInRepo_Should_Return_Empty_List()
    {
        // Arrange
        string repo = "hash3";

        // Act
        IReadOnlyCollection<DateTime> dates = _CommitRepository.ReadAllUniqueDatesInRepo(repo);

        // Assert
        dates.Should().NotBeNull();
        dates.Count.Should().Be(0);
    }

    [Fact]
    public void ReadAllUniqueDatesInRepo_Should_Throw_ArgumentException()
    {
        // Arrange
        string repo = "hash4";

        // Act
        Action action = () => _CommitRepository.ReadAllUniqueDatesInRepo(repo);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ReadAllUniqueAuthorsInRepo_Should_Return_All_Authors_In_Repo()
    {
        // Arrange
        string repo = "hash1";

        // Act
        IReadOnlyCollection<string> authors = _CommitRepository.ReadAllUniqueAuthorsInRepo(repo);

        // Assert
        authors.Should().NotBeNull();
        authors.Count.Should().Be(3);
        authors.Should().BeEquivalentTo(new List<string>()
        {
            "hash1",
            "hash2",
            "hash3",
        });
    }

    [Fact]
    public void ReadAllUniqueAuthorsInRepo_Should_Return_Empty_List()
    {
        // Arrange
        string repo = "hash3";

        // Act
        IReadOnlyCollection<string> authors = _CommitRepository.ReadAllUniqueAuthorsInRepo(repo);

        // Assert
        authors.Should().NotBeNull();
        authors.Count.Should().Be(0);
    }

    [Fact]
    public void ReadAllUniqueAuthorsInRepo_Should_Throw_ArgumentException()
    {
        // Arrange
        string repo = "hash4";

        // Act
        Action action = () => _CommitRepository.ReadAllUniqueAuthorsInRepo(repo);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ReadAllInRepoOnDate_Should_Return_All_Commits_In_Repo_On_Date()
    {
        // Arrange
        string repo = "hash1";
        DateTime date = new DateTime(2022, 10, 28);

        // Act
        IReadOnlyCollection<CommitDTO> commits = _CommitRepository.ReadAllInRepoOnDate(repo, date);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(4);
        commits.Should().BeEquivalentTo(new List<CommitDTO>()
        {
            new CommitDTO("hash1", "message1", new DateTime(2022, 10, 28), "hash1", "hash1"),
            new CommitDTO("hash2", "message2", new DateTime(2022, 10, 28), "hash2", "hash1"),
            new CommitDTO("hash3", "message3", new DateTime(2022, 10, 28), "hash2", "hash1"),
            new CommitDTO("hash4", "message4", new DateTime(2022, 10, 28), "hash3", "hash1"),
        });
    }

    [Fact]
    public void ReadAllInRepoOnDate_Should_Return_Empty_List()
    {
        // Arrange
        string repo = "hash1";
        DateTime date = new DateTime(2022, 11, 2);

        // Act
        IReadOnlyCollection<CommitDTO> commits = _CommitRepository.ReadAllInRepoOnDate(repo, date);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(0);
    }

    [Fact]
    public void ReadAllInRepoOnDate_Should_Throw_ArgumentException()
    {
        // Arrange
        string repo = "hash4";
        DateTime date = new DateTime(2022, 11, 2);

        // Act
        Action action = () => _CommitRepository.ReadAllInRepoOnDate(repo, date);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ReadAllInRepoOnDateByAuthor_Should_Return_All_Commits_In_Repo_On_Date_By_Author()
    {
        // Arrange
        string repo = "hash1";
        DateTime date = new DateTime(2022, 10, 28);
        string author = "hash1";

        // Act
        IReadOnlyCollection<CommitDTO> commits = _CommitRepository.ReadAllInRepoOnDateByAuthor(repo, date, author);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(1);
        commits.Should().BeEquivalentTo(new List<CommitDTO>()
        {
            new CommitDTO("hash1", "message1", new DateTime(2022, 10, 28), "hash1", "hash1"),
        });
    }

    [Fact]
    public void ReadAllInRepoOnDateByAuthor_Should_Return_Empty_List()
    {
        // Arrange
        string repo = "hash1";
        DateTime date = new DateTime(2022, 11, 1);
        string author = "hash1";

        // Act
        IReadOnlyCollection<CommitDTO> commits = _CommitRepository.ReadAllInRepoOnDateByAuthor(repo, date, author);

        // Assert
        commits.Should().NotBeNull();
        commits.Count.Should().Be(0);
    }

    [Fact]
    public void ReadAllInRepoOnDateByAuthor_Should_Throw_ArgumentException_With_Missing_Repo_Message()
    {
        // Arrange
        string repo = "hash4";
        DateTime date = new DateTime(2022, 11, 1);
        string author = "hash1";

        // Act
        Action action = () => _CommitRepository.ReadAllInRepoOnDateByAuthor(repo, date, author);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Repo does not exist in database.");
    }

    [Fact]
    public void ReadAllInRepoOnDateByAuthor_Should_Throw_ArgumentException_With_Missing_Author_Message()
    {
        // Arrange
        string repo = "hash1";
        DateTime date = new DateTime(2022, 11, 1);
        string author = "hash10";

        // Act
        Action action = () => _CommitRepository.ReadAllInRepoOnDateByAuthor(repo, date, author);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Author does not exist in database.");
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}