namespace GitInsight.Infrastructure;

public class Repo
{
    public int Hash { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Commit> Commits { get; set; }

    public Repo(int hash, string name)
    {
        Hash = hash;
        Name = name;
        Commits = new List<Commit>();
    }
}