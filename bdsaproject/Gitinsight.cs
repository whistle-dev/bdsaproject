namespace app;

public class GitInsight
{
    private IRepository _repo;

    public char Mode;

    public GitInsight(String path, char mode) : this(new Repository(@path), mode) {
        printCommits();
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

    public void printCommits()
    {
        if (Mode == 'f')
        {
            foreach (var commit in getCommits())
            {
                Console.WriteLine(commit.Value + " " + commit.Key.ToString("dd/MM/yyyy"));
            }
        }
        else if (Mode == 'a')
        {
            foreach (var author in getCommits())
            {
                Console.WriteLine(author.Key);
                foreach (var commit in author.Value)
                {
                    Console.WriteLine("".PadLeft(5) + commit.Value + " " + commit.Key.ToString("dd/MM/yyyy"));
                }
                Console.WriteLine();
            }

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
            throw new ArgumentException("Invalid mode");
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