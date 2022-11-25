namespace GitInsight.API;

public class Connection
{
    private readonly ICommitRepository _commitRepository;
    private readonly IRepoRepository _repoRepository;
    public Connection(ICommitRepository commitRepository, IRepoRepository repoRepository)
    {
        _commitRepository = commitRepository;
        _repoRepository = repoRepository;
    }

    public async Task<List<CommitDTO>> FetchCommitsAsync(string repoPath, IRepository repo)
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

        repo.Dispose();

        return (List<CommitDTO>)commitList;
    }
}