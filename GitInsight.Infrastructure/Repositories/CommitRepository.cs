namespace GitInsight.Infrastructure;

public class CommitRepository : ICommitRepository
{
    private readonly GitinsightContext _context;

    public CommitRepository(GitinsightContext context) => _context = context;

    public void Create(CommitCreateDTO commit)
    {
        var isCommitInDb = _context.Commits.Any(c => c.Hash == commit.Hash);

        var isRepoInDb = _context.Repos.Any(r => r.Hash == commit.RepoHash);

        var isAuthorInDb = _context.Authors.Any(a => a.Hash == commit.AuthorHash);

        if (isCommitInDb)
        {
            throw new ArgumentException("Commit already exists in database.");
        }
        else if (!isRepoInDb)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else if (!isAuthorInDb)
        {
            throw new ArgumentException("Author does not exist in database.");
        }
        else
        {
            var commitEntity = new Commit(commit.Hash, commit.Message, commit.Date) { AuthorHash = commit.AuthorHash, RepoHash = commit.RepoHash };

            _context.Commits.Add(commitEntity);
            _context.SaveChanges();
        }
    }

    public CommitDTO? Find(string hash)
    {
        var commit = from c in _context.Commits
                     where c.Hash == hash
                     select new CommitDTO(c.Hash, c.Message, c.Date, c.AuthorHash!, c.RepoHash!);

        return commit.FirstOrDefault();
    }

    public IReadOnlyCollection<string> ReadAllUniqueAuthorsInRepo(string repo)
    {
        var repoExists = _context.Repos.Any(r => r.Hash == repo);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var authors = from c in _context.Commits
                          where c.RepoHash == repo
                          select c.AuthorHash;

            return authors.Distinct().ToList();
        }
    }

    public IReadOnlyCollection<DateTime> ReadAllUniqueDatesInRepo(string repo)
    {
        var repoExists = _context.Repos.Any(r => r.Hash == repo);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var dates = from c in _context.Commits
                        where c.RepoHash == repo
                        select c.Date;

            return dates.Distinct().ToList();
        }
    }

    public IReadOnlyCollection<CommitDTO> ReadAllInRepo(string repo)
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
                          select new CommitDTO(c.Hash, c.Message, c.Date, c.AuthorHash!, c.RepoHash!);

            return commits.ToList();
        }
    }

    public IReadOnlyCollection<CommitDTO> ReadAllInRepoOnDate(string repo, DateTime date)
    {
        var repoExists = _context.Repos.Any(r => r.Hash == repo);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var commits = from c in _context.Commits
                        where c.RepoHash == repo && c.Date == date
                        select new CommitDTO(c.Hash, c.Message, c.Date, c.AuthorHash!, c.RepoHash!);

            return commits.ToList();
        }
    }

    public IReadOnlyCollection<CommitDTO> ReadAllInRepoOnDateByAuthor(string repo, DateTime date, string author)
    {
        var repoExists = _context.Repos.Any(r => r.Hash == repo);

        var authorExists = _context.Authors.Any(a => a.Hash == author);

        if (!repoExists)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else if (!authorExists)
        {
            throw new ArgumentException("Author does not exist in database.");
        }
        else
        {
            var commits = from c in _context.Commits
                        where c.RepoHash == repo && c.Date == date && c.AuthorHash == author
                        select new CommitDTO(c.Hash, c.Message, c.Date, c.AuthorHash!, c.RepoHash!);

            return commits.ToList();
        }
    }
}