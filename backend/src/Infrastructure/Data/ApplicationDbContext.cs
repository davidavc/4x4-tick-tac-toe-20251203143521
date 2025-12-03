using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.BoardState).IsRequired().HasMaxLength(16);
                entity.Property(g => g.CurrentPlayer).IsRequired();
                entity.Property(g => g.Status).IsRequired();
                entity.Property(g => g.Difficulty).IsRequired();
                entity.Property(g => g.PlayerStartsFirst).IsRequired();
                entity.Property(g => g.CreatedAt).IsRequired();
                entity.Property(g => g.Winner).IsRequired(false);
                entity.Property(g => g.WinningLine).IsRequired(false);
                entity.Property(g => g.CompletedAt).IsRequired(false);
            });
        }
    }
}