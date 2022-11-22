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
}