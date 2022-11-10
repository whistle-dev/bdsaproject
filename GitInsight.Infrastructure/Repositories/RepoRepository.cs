namespace GitInsight.Infrastructure;

public class RepoRepository : IRepoRepository
{
    private readonly GitinsightContext _context;

    public RepoRepository(GitinsightContext context) => _context = context;

    public void Create(RepoCreateDTO repo)
    {
        var isRepoInDb = _context.Repos.Any(r => r.Hash == repo.Hash);

        if (isRepoInDb)
        {
            throw new ArgumentException("Repo already exists in database.");
        }
        else
        {
            var repoEntity = new Repo(repo.Hash, repo.Name);

            _context.Repos.Add(repoEntity);
            _context.SaveChanges();
        }
    }

    public RepoDTO? Find(string hash)
    {
        var repo = from r in _context.Repos
                   where r.Hash == hash
                   select new RepoDTO(r.Hash, r.Name);

        return repo.FirstOrDefault();
    }

    public IReadOnlyCollection<RepoDTO> ReadAll()
    {
        var repos = from r in _context.Repos
                    select new RepoDTO(r.Hash, r.Name);

        return repos.ToList();
    }
}