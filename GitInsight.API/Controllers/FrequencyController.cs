namespace GitInsight.API;

[ApiController]
[Route("[controller]")]
public class FrequencyController : ControllerBase
{

    //Get all commits from a repository
    [Route("{username}/{reponame}")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, int>))]
    public async Task<ActionResult<string>> Get(string username, string reponame)
    {
        try
        {
            var git = await GitInsight.BuildGitInsightAsync(username, reponame, 'f');
            var commits = git.getCommitsFrequency();
            git.removeRepo();
            return commits == null ? NotFound(): Ok(commits);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}