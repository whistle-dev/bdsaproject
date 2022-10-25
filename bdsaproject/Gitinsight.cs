namespace Gitinsight;

public class Gitinsight
{
    private Repository repo;

    public Gitinsight(String path)
    {
        repo = new Repository(@path);
    }


    public void getCommits(string mode)
    {
        if (mode == "1")
        {
            getCommitsFrequency();
        }
        else if (mode == "2")
        {
            getCommitsAuthor();
        }
    }


    public string getRepoName()
    {
        return repo.Info.WorkingDirectory;
    }

    private void getCommitsFrequency()
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

        foreach (var commit in commitsByDate)
        {
            Console.WriteLine(commit.Value + " " + commit.Key.ToString("dd/MM/yyyy"));
        }
    }

    private void getCommitsAuthor()
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

        foreach (var author in commitsByAuthor)
        {
            Console.WriteLine(author.Key);
            foreach (var commit in author.Value)
            {
                Console.WriteLine(commit.Value + " " + commit.Key.ToString("dd/MM/yyyy"));
            }
            Console.WriteLine();
        }
    }

}