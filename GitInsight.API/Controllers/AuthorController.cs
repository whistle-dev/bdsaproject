namespace GitInsight.API;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    //Get all dates that contains commits with the author
    
    [Route("{username}/{reponame}")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, Dictionary<string, int>>))]
    public async Task<ActionResult<string>> Get(string username, string reponame)
    {
        try
        {
            string url = $"https://github.com/{username}/{reponame}";
            var git = await GitInsight.BuildGitInsightAsync(url, 'a');
            var commits = git.getCommitsAuthor();
            git.removeRepo();
            return commits == null ? NotFound(): Ok(commits);

        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
        

    //Get specific authors with their commits for each day
    [Route("{username}/{reponame}/{author}")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, int>))]
    public async Task<ActionResult<string>> Get(string username, string reponame, string author)
    {
        try
        {
            string url = $"https://github.com/{username}/{reponame}";
            var git = await GitInsight.BuildGitInsightAsync(url, 'a');
            var commits = git.getCommitsFromAuthor(author);
            git.removeRepo();
            return commits == null ? NotFound(): Ok(commits);
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

}