namespace GitInsight.Core
{
    public interface IRepoRepository
    {
        void Create(RepoCreateDTO repo);
        RepoDTO? Find(string hash);
        IReadOnlyCollection<RepoDTO> ReadAll();
    }
}