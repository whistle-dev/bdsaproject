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
                r.HasKey(r => r.Hash);
                r.Property(r => r.Hash).ValueGeneratedNever();

                r.HasIndex(r => r.Hash).IsUnique();

                r.HasMany(x => x.Commits).WithOne(c => c.Repo).HasForeignKey(c => c.RepoHash);

                r.Property(r => r.Hash)
                    .IsRequired();

                r.Property(r => r.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<Commit>(c => {
                c.HasKey(c => c.Hash);
                c.Property(c => c.Hash).ValueGeneratedNever();
                
                c.HasIndex(c => c.Hash).IsUnique();

                c.HasOne(c => c.Repo).WithMany(r => r.Commits).HasForeignKey(c => c.RepoHash);

                c.HasIndex(c => c.RepoHash);

                c.Property(c => c.Hash)
                    .IsRequired();

                c.Property(c => c.Message)
                    .IsRequired();

                c.Property(c => c.Date)
                    .IsRequired();

                c.Property(c => c.Author)
                    .IsRequired();

                c.Property(c => c.RepoHash)
                    .IsRequired();
            });
        }
    }
}