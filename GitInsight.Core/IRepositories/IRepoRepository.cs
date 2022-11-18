namespace GitInsight.Core
{
    public interface IRepoRepository
    {
        void Create(RepoCreateDTO repo);
        RepoDTO? Find(string path);
        void Update(RepoUpdateDTO repo);
        IReadOnlyCollection<RepoDTO> ReadAll();
    }
}