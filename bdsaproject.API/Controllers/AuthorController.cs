namespace app.API;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    [Route("{username}/{reponame}/{author}")]
    [HttpGet]
    public async Task<ActionResult<string>> Get(string username, string reponame, string author)
    {
        string url = $"https://github.com/{username}/{reponame}";
        var git = new GitInsight(url, 'a');
        var commits = git.getCommitsFromAuthor(author);
        git.removeRepo();
        return Ok(commits);
    }
}