namespace GitInsight.Core
{
    public interface IRepoRepository
    {
        void Create(RepoCreateDTO repo);
        RepoDTO? Find(int hash);
        IReadOnlyCollection<RepoDTO> ReadAll();
    }
}