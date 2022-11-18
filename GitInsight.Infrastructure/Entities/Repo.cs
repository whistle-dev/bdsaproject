namespace GitInsight.Infrastructure;

public class Repo
{
    public string Path { get; set; }
    public string? LatestCommitSha { get; set; }
    public virtual ICollection<Commit> Commits { get; set; }

    public Repo(string path)
    {
        Path = path;
        Commits = new List<Commit>();
    }
}