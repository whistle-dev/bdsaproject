namespace GitInsight.Infrastructure;

public class CommitRepository : ICommitRepository
{
    private readonly GitinsightContext _context;

    public CommitRepository(GitinsightContext context) => _context = context;

    public void Create(CommitCreateDTO commit)
    {
        var isCommitInDb = _context.Commits.Any(c => c.Hash == commit.Hash);

        var isRepoInDb = _context.Repos.Any(r => r.Hash == commit.RepoHash);

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
            var commitEntity = new Commit(commit.Hash, commit.Message, commit.Date, commit.Author) { RepoHash = commit.RepoHash };

            _context.Commits.Add(commitEntity);
            _context.SaveChanges();
        }
    }

    public CommitDTO? Find(int hash)
    {
        var commit = from c in _context.Commits
                     where c.Hash == hash
                     select new CommitDTO(c.Hash, c.Message, c.Date, c.Author, c.RepoHash!);

        return commit.FirstOrDefault();
    }

    public IReadOnlyCollection<CommitDTO> ReadAllInRepo(int repo)
    {
        var repoExists = _context.Repos.Any(r => r.Hash == repo);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var commits = from c in _context.Commits
                          where c.RepoHash == repo
                          select new CommitDTO(c.Hash, c.Message, c.Date, c.Author, c.RepoHash!);

            return commits.ToList();
        }
    }
}