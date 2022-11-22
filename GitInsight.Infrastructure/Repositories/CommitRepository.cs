namespace GitInsight.Infrastructure;

public class CommitRepository : ICommitRepository
{
    private readonly GitInsightContext _context;

    public CommitRepository(GitInsightContext context) => _context = context;

    public async Task CreateAsync(CommitCreateDTO commit)
    {
        var isCommitInDb = await _context.Commits.AnyAsync(c => c.Sha == commit.Sha);

        var isRepoInDb = await _context.Repos.AnyAsync(r => r.Path == commit.RepoPath);

        if (isCommitInDb)
        {
            return;
        }
        else if (!isRepoInDb)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var commitEntity = new Commit(commit.Sha, commit.Message, commit.Date, commit.Author, commit.RepoPath);

            _context.Commits.Add(commitEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CommitDTO?> FindAsync(string sha)
    {
        var commit = from c in _context.Commits
                     where c.Sha == sha
                     select new CommitDTO(c.Sha, c.Message, c.Date, c.Author, c.RepoPath);

        return await commit.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CommitDTO>> ReadAllInRepoAsync(string repoPath)
    {
        var repoExists = await _context.Repos.AnyAsync(r => r.Path == repoPath);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var commits = from c in _context.Commits
                          where c.RepoPath == repoPath
                          select new CommitDTO(c.Sha, c.Message, c.Date, c.Author, c.RepoPath);

            return await commits.ToListAsync();
        }
    }
}