namespace GitInsight.API.Integration.Tests;

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

            services.AddDbContext<GitInsightContext>(options =>
            {
                options.UseSqlServer("Filename=:memory:");
            });
        });

        builder.UseEnvironment("Development");
    }
}