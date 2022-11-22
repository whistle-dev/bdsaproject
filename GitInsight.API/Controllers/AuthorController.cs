namespace GitInsight.API;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    //Get all dates that contains commits with the author
    [Route("{username}/{reponame}")]
    [HttpGet]
    public async Task<ActionResult<string>> Get(string username, string reponame)
    {
        string url = $"https://github.com/{username}/{reponame}";
        var git = new GitInsight(url, 'a');
        await git.fetchCommitsFromDb();
        var commits = git.getCommitsAuthor();
        git.removeRepo();
        return Ok(commits);
    }

    //Get specific authors with their commits for each day
    [Route("{username}/{reponame}/{author}")]
    [HttpGet]
    public async Task<ActionResult<string>> Get(string username, string reponame, string author)
    {
        string url = $"https://github.com/{username}/{reponame}";
        var git = new GitInsight(url, 'a');
        await git.fetchCommitsFromDb();
        var commits = git.getCommitsFromAuthor(author);
        git.removeRepo();
        return Ok(commits);
    }

}