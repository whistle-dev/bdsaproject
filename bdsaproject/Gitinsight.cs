namespace app;

public class GitInsight
{
    private Repository repo;

    public char Mode;

    public GitInsight(String path, char mode)
    {
        repo = new Repository(@path);
        Mode = mode;
        getCommits();


    }
    public void getCommits()
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

    private Dictionary<DateTime, int> getCommitsFrequency()
    {
        var commits = repo.Commits;
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

    public Dictionary<string, Dictionary<DateTime, int>> getCommitsAuthor()
    {
        var commits = repo.Commits;
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