namespace app;

public class Connection
{
    
    private readonly GitinsightContext _context;
    private readonly AuthorRepository _authorRepository;
    private readonly CommitRepository _commitRepository;
    private readonly RepoRepository _repoRepository;

    public Connection()
    {
        var factory = new GitInsightContextFactory();
        _context = factory.CreateDbContext(new string[0]);
        _authorRepository = new AuthorRepository(_context);
        _commitRepository = new CommitRepository(_context);
        _repoRepository = new RepoRepository(_context);
    }

    // public List<Commit> fetchCommits(IRepository repo)
    // {
    //     if (_repoRepository.Find(repo.GetHashCode()) == null)
    //     {
    //         foreach (var commit in repo.Commits)
    //         {
    //             if (_authorRepository.Find(commit.Author.))
    //         }
    //     }
    // }
}