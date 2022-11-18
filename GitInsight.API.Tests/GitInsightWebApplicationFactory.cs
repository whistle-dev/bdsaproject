namespace GitInsight.API.Tests;

public class GitInsightWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
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
            var tempConnectionString = configuration.GetConnectionString("GitInsight")!;

            var connectionString = Regex.Replace(tempConnectionString, "Database=[^;]+;", "Database="+Guid.NewGuid().ToString()+";");


            services.AddDbContext<GitInsightContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<GitInsightContext>();
        await context.Database.EnsureCreatedAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<GitInsightContext>();
        await context.Database.EnsureDeletedAsync();
    }
}