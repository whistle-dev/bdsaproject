namespace GitInsight.Infrastructure;

public class Commit
{
    public string Sha { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public string Author { get; set; }
    public string RepoPath { get; set; }
    public virtual Repo? Repo { get; set; }

    public Commit(string sha, string message, DateTime date, string author, string repoPath)
    {
        Sha = sha;
        Message = message;
        Date = date;
        Author = author;
        RepoPath = repoPath;
    }
}