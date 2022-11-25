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

    async public static Task<GitInsight> BuildGitInsightAsync(string repoAuthor, string repoName, char mode, ICommitRepository commits, IRepoRepository repos)
    {
        string url = $"https://github.com/{repoAuthor}/{repoName}";
        
        var localPath = Path.Combine(Path.GetTempPath(), "GitInsight", Guid.NewGuid().ToString());
        var repoPath = Path.Combine(repoAuthor, repoName);

        Console.WriteLine("Cloning repository ...");
        if (Directory.Exists(localPath))
        {
            removeRepo(localPath);
        }
        Repository.Clone(url, localPath);
        Console.WriteLine("Repository cloned.");
        
        var repoFromPath = new Repository(localPath);
        var connection = new Connection(commits, repos);

        var updatedcommits = await connection.FetchCommitsAsync(repoPath, repoFromPath);
        repoFromPath.Dispose();

        return new GitInsight(updatedcommits, mode, localPath);
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
            var date = commit.Date.ToString("dd-MM-yyyy");
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
            var date = commit.Date.ToString("dd-MM-yyyy");
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