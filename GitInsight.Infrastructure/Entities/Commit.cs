namespace GitInsight.Infrastructure;

public class Commit
{
    public int Hash { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public string Author { get; set; }
    public int RepoHash { get; set; }
    public virtual Repo? Repo { get; set; }

    public Commit(int hash, string message, DateTime date, string author)
    {
        Hash = hash;
        Message = message;
        Date = date;
        Author = author;
    }
}