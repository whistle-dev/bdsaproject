namespace GitInsight.Infrastructure;

public class Author
{
    public string Hash { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Commit> Commits { get; set; }

    public Author(string hash, string name, string email)
    {
        Hash = hash;
        Name = name;
        Email = email;
        Commits = new List<Commit>();
    }
}