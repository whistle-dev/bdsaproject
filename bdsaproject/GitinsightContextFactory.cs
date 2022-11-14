namespace app;

public class GitInsightContextFactory : IDesignTimeDbContextFactory<GitinsightContext>
{
    public GitinsightContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("GitInsight");

        var optionsBuilder = new DbContextOptionsBuilder<GitinsightContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new GitinsightContext(optionsBuilder.Options);
    }
}