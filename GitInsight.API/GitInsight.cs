namespace GitInsight.API;

public class GitInsight
{
    private List<CommitDTO> commits;
    private string path;
    public char Mode;

    public GitInsight(List<CommitDTO> _commits, char mode, string path)
    {
        commits = _commits;
        commits.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

        if (mode != 'a' && mode != 'f')
        {
            throw new ArgumentException("Invalid mode");
        }
        Mode = mode;
        this.path = path;
    }

    async public static Task<GitInsight> BuildGitInsightAsync(string url, char mode)
    {
        string[] urlSplitted = url.Split("/");
        string repoAuthor = urlSplitted[urlSplitted.Length - 2];
        string repoName = urlSplitted[urlSplitted.Length - 1];
        
        var path = Path.Combine(Path.GetTempPath(), repoAuthor, repoName);

        Console.WriteLine("Cloning repository ...");
        if (Directory.Exists(path))
        {
            removeRepo(path);
        }
        Repository.Clone(url, path);
        Console.WriteLine("Repository cloned.");
        
        var repoFromPath = new Repository(path);
        var connection = new Connection();

        var commits = await connection.fetchCommits(repoFromPath);
        repoFromPath.Dispose();

        return new GitInsight(commits, mode, path);
    }

    public static void removeRepo(string path)
    {
        try
        {
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

    public void removeRepo()
    {
        removeRepo(path);
    }

    public Dictionary<string, int> getCommitsFromAuthor(string author)
    {
        var commits = getCommitsAuthor()[author];
        return commits;
    }

    public Dictionary<string, int> getCommitsFrequency()
    {
        var commitsByDate = new Dictionary<string, int>();
        foreach (var commit in commits)
        {
            var date = commit.Date.ToShortDateString();
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

    public Dictionary<string, Dictionary<string, int>> getCommitsAuthor()
    {
        var commitsByAuthor = new Dictionary<string, Dictionary<string, int>>();
        foreach (var commit in commits)
        {
            var date = commit.Date.ToShortDateString();
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
                commitsByAuthor[author] = new Dictionary<string, int> { { date, 1 } };
            }
        }

        return commitsByAuthor;
    }

    public void printCommits()
    {
        if (Mode == 'f')
        {
            foreach (var commit in getCommitsFrequency())
            {
                Console.WriteLine(commit.Value + " " + commit.Key);
            }
        }
        else if (Mode == 'a')
        {
            foreach (var author in getCommitsAuthor())
            {
                Console.WriteLine(author.Key);
                foreach (var commit in author.Value)
                {
                    Console.WriteLine("".PadLeft(5) + commit.Value + " " + commit.Key);
                }
                Console.WriteLine();
            }

        }
    }
}