namespace GitInsight.Infrastructure;

public class Commit
{
    public string Hash { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public string? AuthorHash { get; set; }
    public string? RepoHash { get; set; }
    public virtual Author? Author { get; set; }
    public virtual Repo? Repo { get; set; }

    public Commit(string hash, string message, DateTime date)
    {
        Hash = hash;
        Message = message;
        Date = date;
    }
}