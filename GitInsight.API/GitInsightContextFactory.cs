namespace GitInsight.API;

public class GitInsightContextFactory : IDesignTimeDbContextFactory<GitInsightContext>
{
    public GitInsightContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("GitInsight");

        var optionsBuilder = new DbContextOptionsBuilder<GitInsightContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new GitInsightContext(optionsBuilder.Options);
    }
}