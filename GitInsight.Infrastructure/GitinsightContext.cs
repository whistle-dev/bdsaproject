namespace GitInsight.Infrastructure
{
    public class GitinsightContext : DbContext
    {
        public GitinsightContext(DbContextOptions<GitinsightContext> options) : base(options)
        {
        }

        public DbSet<Commit> Commits => Set<Commit>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Repo> Repos => Set<Repo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(a => {
                a.HasKey(x => x.Hash);

                a.HasIndex(a => a.Hash).IsUnique();

                a.HasMany(x => x.Commits).WithOne(c => c.Author).HasForeignKey(c => c.AuthorHash);

                a.Property(a => a.Hash)
                    .IsRequired();

                a.Property(a => a.Name)
                    .IsRequired();

                a.Property(a => a.Email)
                    .IsRequired();
            });
            
            modelBuilder.Entity<Repo>(r => {
                r.HasKey(x => x.Hash);

                r.HasIndex(r => r.Hash).IsUnique();

                r.HasMany(x => x.Commits).WithOne(c => c.Repo).HasForeignKey(c => c.RepoHash);

                r.Property(r => r.Hash)
                    .IsRequired();

                r.Property(r => r.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<Commit>(c => {
                c.HasKey(x => x.Hash);

                c.HasIndex(c => c.Hash).IsUnique();

                c.HasOne(c => c.Author).WithMany(a => a.Commits).HasForeignKey(c => c.AuthorHash);
                c.HasOne(c => c.Repo).WithMany(r => r.Commits).HasForeignKey(c => c.RepoHash);

                c.HasIndex(c => c.AuthorHash);
                c.HasIndex(c => c.RepoHash);

                c.Property(c => c.Hash)
                    .IsRequired();

                c.Property(c => c.Message)
                    .IsRequired();

                c.Property(c => c.Date)
                    .IsRequired();

                c.Property(c => c.AuthorHash)
                    .IsRequired();

                c.Property(c => c.RepoHash)
                    .IsRequired();
            });
        }
    }
}