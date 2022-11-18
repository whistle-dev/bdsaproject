namespace GitInsight.Infrastructure;

public class CommitRepository : ICommitRepository
{
    private readonly GitInsightContext _context;

    public CommitRepository(GitInsightContext context) => _context = context;

    public void Create(CommitCreateDTO commit)
    {
        var isCommitInDb = _context.Commits.Any(c => c.Sha == commit.Sha);

        var isRepoInDb = _context.Repos.Any(r => r.Path == commit.RepoPath);

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
            _context.SaveChanges();
        }
    }

    public CommitDTO? Find(string sha)
    {
        var commit = from c in _context.Commits
                     where c.Sha == sha
                     select new CommitDTO(c.Sha, c.Message, c.Date, c.Author, c.RepoPath);

        return commit.FirstOrDefault();
    }

    public CommitDTO? FindLatestInRepo(string repoPath)
    {
        var commit = from c in _context.Commits
                     where c.RepoPath == repoPath
                     orderby c.Date descending
                     select new CommitDTO(c.Sha, c.Message, c.Date, c.Author, c.RepoPath);

        return commit.FirstOrDefault();
    }

    public IReadOnlyCollection<CommitDTO> ReadAllInRepo(string repoPath)
    {
        var repoExists = _context.Repos.Any(r => r.Path == repoPath);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var commits = from c in _context.Commits
                          where c.RepoPath == repoPath
                          select new CommitDTO(c.Sha, c.Message, c.Date, c.Author, c.RepoPath);

            return commits.ToList();
        }
    }
}