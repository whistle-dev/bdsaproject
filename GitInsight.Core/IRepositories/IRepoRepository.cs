namespace GitInsight.Core
{
    public interface IRepoRepository
    {
        Task CreateAsync(RepoCreateDTO repo);
        Task<RepoDTO?> FindAsync(string path);
        Task UpdateAsync(RepoUpdateDTO repo);
        Task<IReadOnlyCollection<RepoDTO>> ReadAllAsync();
    }
}