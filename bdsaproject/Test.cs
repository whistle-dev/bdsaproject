namespace bdsaproject;

class Test
{
    private readonly GitinsightContext _context;

    private readonly RepoRepository _repoRepository;
    private readonly CommitRepository _commitRepository;
    private readonly AuthorRepository _authorRepository;

    public Test()
    {
        var connection = new SqlConnection("Server=localhost;Database=bdsaproject;User Id=sa;Password=SMBbdsaproject1;");
        connection.Open();

        var optionsBuilder = new DbContextOptionsBuilder<GitinsightContext>();
        optionsBuilder.UseSqlServer(connection);

        _context = new GitinsightContext(optionsBuilder.Options);
        _context.Database.EnsureCreated();

        _repoRepository = new RepoRepository(_context);
        _commitRepository = new CommitRepository(_context);
        _authorRepository = new AuthorRepository(_context);
    }
}