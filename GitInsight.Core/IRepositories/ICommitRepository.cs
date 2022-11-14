namespace GitInsight.Core
{
    public interface ICommitRepository
    {
        void Create(CommitCreateDTO commit);
        CommitDTO? Find(int hash);
        IReadOnlyCollection<CommitDTO> ReadAllInRepo(int repo);
    }

}