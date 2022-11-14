namespace app;

public class Connection
{
    private readonly GitinsightContext _context;
    private readonly CommitRepository _commitRepository;
    private readonly RepoRepository _repoRepository;

    public Connection()
    {
        var factory = new GitInsightContextFactory();
        _context = factory.CreateDbContext(new string[0]);
        _context.Database.EnsureCreated();
        _commitRepository = new CommitRepository(_context);
        _repoRepository = new RepoRepository(_context);
        _context.SaveChanges();
    }

    public List<CommitDTO> fetchCommits(IRepository repo)
    {
        var repoHash = repo.GetHashCode();
        if (_repoRepository.Find(repoHash) == null || _commitRepository.ReadAllInRepo(repoHash).Count() < repo.Commits.Count())
        {
            _repoRepository.Create(new RepoCreateDTO(repoHash, repo.Info.WorkingDirectory));
            foreach (var commit in repo.Commits)
            {
                _commitRepository.Create(new CommitCreateDTO(commit.GetHashCode(),
                                                                commit.Message,
                                                                commit.Author.When.Date,
                                                                commit.Author.Name,
                                                                repoHash
                                                            ));
            }
        }

        return (List<CommitDTO>)_commitRepository.ReadAllInRepo(repoHash);
    }
}