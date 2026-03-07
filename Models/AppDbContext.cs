using Microsoft.EntityFrameworkCore;

namespace Wattpadmock1.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Story)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Story>()
                .HasOne(s => s.Author)
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}