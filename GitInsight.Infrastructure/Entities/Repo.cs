namespace GitInsight.Infrastructure;

public class Repo
{
    public string Hash { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Commit> Commits { get; set; }

    public Repo(string hash, string name)
    {
        Hash = hash;
        Name = name;
        Commits = new List<Commit>();
    }
}