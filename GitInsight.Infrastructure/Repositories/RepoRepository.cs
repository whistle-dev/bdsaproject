namespace GitInsight.Infrastructure;

public class RepoRepository : IRepoRepository
{
    private readonly GitinsightContext _context;

    public RepoRepository(GitinsightContext context) => _context = context;

    public void Create(RepoCreateDTO repo)
    {
        var isRepoInDb = _context.Repos.Any(r => r.Path == repo.Path);

        if (isRepoInDb)
        {
            throw new ArgumentException("Repo already exists in database.");
        }
        else
        {
            var repoEntity = new Repo(repo.Path) { LatestCommitSha = repo.LatestCommitSha };

            _context.Repos.Add(repoEntity);
            _context.SaveChanges();
        }
    }

    public RepoDTO? Find(string path)
    {
        var repo = from r in _context.Repos
                   where r.Path == path
                   select new RepoDTO(r.Path, r.LatestCommitSha);

        return repo.FirstOrDefault();
    }

    public void Update(RepoUpdateDTO repo)
    {
        var isRepoInDb = _context.Repos.Any(r => r.Path == repo.Path);

        if (!isRepoInDb)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var repoEntity = _context.Repos.First(r => r.Path == repo.Path);

            repoEntity.LatestCommitSha = repo.LatestCommitSha;

            _context.SaveChanges();
        }
    }

    public IReadOnlyCollection<RepoDTO> ReadAll()
    {
        var repos = from r in _context.Repos
                    select new RepoDTO(r.Path, r.LatestCommitSha);

        return repos.ToList();
    }
}