namespace GitInsight.Infrastructure;

public class RepoRepository : IRepoRepository
{
    private readonly GitInsightContext _context;

    public RepoRepository(GitInsightContext context) => _context = context;

    public async Task CreateAsync(RepoCreateDTO repo)
    {
        var isRepoInDb = await _context.Repos.AnyAsync(r => r.Path == repo.Path);

        if (isRepoInDb)
        {
            throw new ArgumentException("Repo already exists in database.");
        }
        else
        {
            var repoEntity = new Repo(repo.Path) { LatestCommitSha = repo.LatestCommitSha };

            _context.Repos.Add(repoEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<RepoDTO?> FindAsync(string path)
    {
        var repo = from r in _context.Repos
                   where r.Path == path
                   select new RepoDTO(r.Path, r.LatestCommitSha);

        return await repo.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(RepoUpdateDTO repo)
    {
        var isRepoInDb = await _context.Repos.AnyAsync(r => r.Path == repo.Path);

        if (!isRepoInDb)
        {
            throw new ArgumentException("Repo does not exist in database.");
        }
        else
        {
            var repoEntity = await _context.Repos.FirstAsync(r => r.Path == repo.Path);

            repoEntity.LatestCommitSha = repo.LatestCommitSha;

            await _context.SaveChangesAsync();
        }
    }

    public async Task<IReadOnlyCollection<RepoDTO>> ReadAllAsync()
    {
        var repos = from r in _context.Repos
                    select new RepoDTO(r.Path, r.LatestCommitSha);

        return await repos.ToListAsync();
    }
}