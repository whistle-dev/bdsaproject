var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var connectionString = configuration.GetConnectionString("GitInsight");

builder.Services.AddDbContext<GitInsightContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepoRepository, RepoRepository>();
builder.Services.AddScoped<ICommitRepository, CommitRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "GitInsightCorsPolicy",
                      policy  =>
                      {
                          policy.WithOrigins("https://localhost:7243")
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyOrigin();
                      });
});

var app = builder.Build();

app.UseCors("GitInsightCorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }