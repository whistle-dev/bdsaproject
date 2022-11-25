namespace GitInsight.API;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ICommitRepository _commitRepository;
    private readonly IRepoRepository _repoRepository;
    public AuthorController(ICommitRepository commitRepository, IRepoRepository repoRepository)
    {
        _commitRepository = commitRepository;
        _repoRepository = repoRepository;
    }

    //Get all dates that contains commits with the author

    [Route("{username}/{reponame}")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, Dictionary<string, int>>))]
    public async Task<ActionResult<string>> Get(string username, string reponame)
    {
        try
        {
            var git = await GitInsight.BuildGitInsightAsync(username, reponame, 'a', _commitRepository, _repoRepository);
            var commits = git.getCommitsAuthor();
            git.removeRepo();
            return commits == null ? NotFound() : Ok(commits);
        }
        catch (Exception)
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
            var git = await GitInsight.BuildGitInsightAsync(username, reponame, 'a', _commitRepository, _repoRepository);
            var commits = git.getCommitsFromAuthor(author);
            git.removeRepo();
            return commits == null ? NotFound() : Ok(commits);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

}