namespace GitInsight.Infrastructure
{
    public class GitinsightContext : DbContext
    {
        public GitinsightContext(DbContextOptions<GitinsightContext> options) : base(options)
        {
        }

        public DbSet<Commit> Commits => Set<Commit>();
        public DbSet<Repo> Repos => Set<Repo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Repo>(r => {
                r.HasKey(r => r.Path);
                r.Property(r => r.Path).ValueGeneratedNever();
                r.Property(r => r.Path).IsRequired();
                r.HasIndex(r => r.Path).IsUnique();

                r.HasMany(x => x.Commits).WithOne(c => c.Repo).HasForeignKey(c => c.RepoPath);
            });

            modelBuilder.Entity<Commit>(c => {
                c.HasKey(c => c.Sha);
                c.Property(c => c.Sha).ValueGeneratedNever();
                c.Property(c => c.Sha).IsRequired();
                c.HasIndex(c => c.Sha).IsUnique();

                c.HasOne(c => c.Repo).WithMany(r => r.Commits).HasForeignKey(c => c.RepoPath);

                c.HasIndex(c => c.RepoPath);
                c.Property(c => c.RepoPath).IsRequired();

                c.Property(c => c.Message).IsRequired();

                c.Property(c => c.Date).IsRequired();

                c.Property(c => c.Author).IsRequired();
            });
        }
    }
}