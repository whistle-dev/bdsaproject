namespace GitInsight.API.Tests;

public class GitInsightWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => 
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GitInsightContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            var connectionString = configuration.GetConnectionString("GitInsight");

            services.AddDbContext<GitInsightContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        });

        builder.UseEnvironment("Development");
    }
}