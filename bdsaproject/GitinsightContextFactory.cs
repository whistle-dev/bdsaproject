namespace app;

public class GitInsightContextFactory : IDesignTimeDbContextFactory<GitinsightContext>
{
    public GitinsightContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var ConnectionString = configuration.GetConnectionString("ConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<GitinsightContext>();
        optionsBuilder.UseSqlServer(ConnectionString);

        return new GitinsightContext(optionsBuilder.Options);
    }
}