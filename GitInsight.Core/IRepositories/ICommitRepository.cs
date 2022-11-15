namespace GitInsight.Core
{
    public interface ICommitRepository
    {
        void Create(CommitCreateDTO commit);
        CommitDTO? Find(string sha);
        IReadOnlyCollection<CommitDTO> ReadAllInRepo(string repoPath);
    }

}