namespace GitInsight.Core
{
    public interface ICommitRepository
    {
        Task CreateAsync(CommitCreateDTO commit);
        Task<CommitDTO?> FindAsync(string sha);
        Task<IReadOnlyCollection<CommitDTO>> ReadAllInRepoAsync(string repoPath);
    }

}