namespace GitInsight.API;

public class Connection
{
    private readonly GitInsightContext _context;
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
        var repoPath = repo.Info.WorkingDirectory;
        var latestCommitSha = repo.Head.Tip.Sha;

        if (_repoRepository.Find(repoPath) == null)
        {
            _repoRepository.Create(new RepoCreateDTO(repo.Info.WorkingDirectory, null));
        }

        if (_repoRepository.Find(repoPath)!.LatestCommitSha == null || _repoRepository.Find(repoPath)!.LatestCommitSha != latestCommitSha)
        {
            Console.WriteLine("Adding commits to database");
            foreach (var commit in repo.Commits)
            {
                _commitRepository.Create(new CommitCreateDTO(commit.Sha,
                                                                commit.Message,
                                                                commit.Author.When.Date,
                                                                commit.Author.Name,
                                                                repoPath
                                                            ));
            }
            _repoRepository.Update(new RepoUpdateDTO(repoPath, latestCommitSha));
        }

        return (List<CommitDTO>)_commitRepository.ReadAllInRepo(repoPath);
    }
}