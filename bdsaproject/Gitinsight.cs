namespace app;

public class GitInsight
{
    private IRepository _repo;

    public char Mode;

    public GitInsight(String url, char mode)
    {
        Console.WriteLine("Cloning repository ...");
        string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\GitInsight";

        if (Directory.Exists(path))
        {
            removeRepo();
        }
        Repository.Clone(url, path);
        Console.WriteLine("Repository cloned.");
        _repo = new Repository(path);
        Mode = mode;
    }

    public GitInsight(IRepository repo, char mode)
    {
        _repo = repo;

        if (mode != 'a' && mode != 'f')
        {
            throw new ArgumentException("Invalid mode");
        }
        Mode = mode;
    }

    public void removeRepo()
    {
        try
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\GitInsight";

            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }
            Directory.Delete(path, true);
        }
        catch (System.Exception)
        {

        }
    }

    public dynamic getCommits()
    {
        if (Mode == 'f')
        {
            return getCommitsFrequency();
        }
        else if (Mode == 'a')
        {
            return getCommitsAuthor();
        }
        else
        {
            return null;
        }
    }

    private Dictionary<DateTime, int> getCommitsFrequency()
    {
        var commits = _repo.Commits;
        var commitsByDate = new Dictionary<DateTime, int>();
        foreach (var commit in commits)
        {
            var date = commit.Committer.When.Date;
            if (commitsByDate.ContainsKey(date))
            {
                commitsByDate[date]++;
            }
            else
            {
                commitsByDate[date] = 1;
            }
        }

        return commitsByDate;
    }

    private Dictionary<string, Dictionary<DateTime, int>> getCommitsAuthor()
    {
        var commits = _repo.Commits;
        var commitsByAuthor = new Dictionary<string, Dictionary<DateTime, int>>();
        foreach (var commit in commits)
        {
            var date = commit.Committer.When.Date;
            var author = commit.Author.Name;
            if (commitsByAuthor.ContainsKey(author))
            {
                var commitsByDate = commitsByAuthor[author];
                if (commitsByDate.ContainsKey(date))
                {
                    commitsByDate[date]++;
                }
                else
                {
                    commitsByDate[date] = 1;
                }
            }
            else
            {
                commitsByAuthor[author] = new Dictionary<DateTime, int> { { date, 1 } };
            }
        }

        return commitsByAuthor;
    }

}