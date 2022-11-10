namespace GitInsight.Core
{
    public interface IAuthorRepository
    {
        void Create(AuthorCreateDTO author);
        AuthorDTO? Find(string hash);
        IReadOnlyCollection<AuthorDTO> ReadAll();
    }
}