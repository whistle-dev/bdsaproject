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

    public async Task fetchCommitsFromDb()
    {
        var repoFromPath = new Repository(path);
        var connection = new Connection();
        commits = await connection.fetchCommits(repoFromPath);
        commits.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        repoFromPath.Dispose();
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

}