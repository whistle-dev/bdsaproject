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

    public async Task<List<CommitDTO>> fetchCommits(string repoPath, IRepository repo)
    {
        var latestCommitSha = repo.Head.Tip.Sha;

        if (await _repoRepository.FindAsync(repoPath) == null)
        {
            await _repoRepository.CreateAsync(new RepoCreateDTO(repoPath, null));
        }

        var repoFromDb = await _repoRepository.FindAsync(repoPath);

        if (repoFromDb!.LatestCommitSha == null || repoFromDb!.LatestCommitSha != latestCommitSha)
        {
            Console.WriteLine("Adding commits to database");
            foreach (var commit in repo.Commits)
            {
                await _commitRepository.CreateAsync(new CommitCreateDTO(commit.Sha,
                                                                commit.Message,
                                                                commit.Author.When.Date,
                                                                commit.Author.Name,
                                                                repoPath
                                                            ));
            }
            await _repoRepository.UpdateAsync(new RepoUpdateDTO(repoPath, latestCommitSha));
        }

        var commitList = await _commitRepository.ReadAllInRepoAsync(repoPath);

        return (List<CommitDTO>)commitList;
    }
}