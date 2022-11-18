namespace GitInsight.API;

public class GitInsight
{
    private List<CommitDTO> commits;
    private string path;
    public char Mode;

    public GitInsight(String url, char mode) : this(new List<CommitDTO>(), mode)
    {
        string[] urlSplitted = url.Split("/");
        string repoAuthor = urlSplitted[urlSplitted.Length - 2];
        string repoName = urlSplitted[urlSplitted.Length - 1];
        
        Console.WriteLine("Cloning repository ...");
        path = Path.Combine(Path.GetTempPath(), repoAuthor, repoName);

        if (Directory.Exists(path))
        {
            removeRepo();
        }
        Repository.Clone(url, path);
        Console.WriteLine("Repository cloned.");
        var repoFromPath = new Repository(path);

        var connection = new Connection();
        commits = connection.fetchCommits(repoFromPath);
        commits.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

        repoFromPath.Dispose();

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
        path = "";
    }

    public void removeRepo()
    {
        try
        {
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\GitInsight";

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

    public Dictionary<DateTime, int> getCommitsFromAuthor(string author)
    {

        var commits = getCommitsAuthor()[author];
        return commits;

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

    public Dictionary<string, Dictionary<string, int>> getDateWithAuthorCommits()
    {
        var commitsByDate = new Dictionary<string, Dictionary<string, int>>();
        foreach (var commit in commits)
        {
            var date = commit.Date.ToShortDateString();
            var author = commit.Author;
            if (commitsByDate.ContainsKey(date))
            {
                var commitsByAuthor = commitsByDate[date];
                if (commitsByAuthor.ContainsKey(author))
                {
                    commitsByAuthor[author]++;
                }
                else
                {
                    commitsByAuthor[author] = 1;
                }
            }
            else
            {
                commitsByDate[date] = new Dictionary<string, int> { { author, 1 } };
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