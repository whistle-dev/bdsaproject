namespace GitInsight.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly GitinsightContext _context;

    public AuthorRepository(GitinsightContext context) => _context = context;

    public void Create(AuthorCreateDTO author)
    {
        var isAuthorInDb = _context.Authors.Any(a => a.Hash == author.Hash);

        if (isAuthorInDb)
        {
            throw new ArgumentException("Author already exists in database.");
        }
        else
        {
            var authorEntity = new Author(author.Hash, author.Name, author.Email);

            _context.Authors.Add(authorEntity);
            _context.SaveChanges();
        }
    }

    public AuthorDTO? Find(string hash)
    {
        var author = from a in _context.Authors
                     where a.Hash == hash
                     select new AuthorDTO(a.Hash, a.Name, a.Email);

        return author.FirstOrDefault();
    }

    public IReadOnlyCollection<AuthorDTO> ReadAll()
    {
        var authors = from a in _context.Authors
                      select new AuthorDTO(a.Hash, a.Name, a.Email);
        
        return authors.ToList();
    }
}