namespace app;

public class GitInsight
{
    private List<CommitDTO> commits;

    public char Mode;
    public GitInsight(String path, char mode) : this(new List<CommitDTO>(), mode)
    {
        var repoFromPath = new Repository(path);
        var connection = new Connection();
        commits = connection.fetchCommits(repoFromPath);
        commits.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
    }

    public GitInsight(List<CommitDTO> _commits, char mode)
    {
        commits = _commits;
        commits.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

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
            foreach (var commit in getCommitsFrequency())
            {
                Console.WriteLine(commit.Value + " " + commit.Key.ToString("dd/MM/yyyy"));
            }
        }
        else if (Mode == 'a')
        {
            foreach (var author in getCommitsAuthor())
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
        var commitsByDate = new Dictionary<DateTime, int>();
        foreach (var commit in commits)
        {
            var date = commit.Date;
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
        var commitsByAuthor = new Dictionary<string, Dictionary<DateTime, int>>();
        foreach (var commit in commits)
        {
            var date = commit.Date;
            var author = commit.Author;
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