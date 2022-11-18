namespace GitInsight.API;

[ApiController]
[Route("[controller]")]
public class FrequencyController : ControllerBase
{

    //Get all commits from a repository
    [Route("{username}/{reponame}")]
    [HttpGet]
    public ActionResult<string> Get(string username, string reponame)
    {
        string url = $"https://github.com/{username}/{reponame}";
        var git = new GitInsight(url, 'f');
        var commits = git.getCommits();
        git.removeRepo();
        return Ok(commits);
    }
}